<template>
<div id="userinfolist">
    <wy-header :titles="titles"></wy-header>
    <wy-grid ref="grid" @handleSearch="searchData" @handleReset="reset" :recordCount="recordCount" :loading="loading">
        <template slot="button">
            <el-button type="text" icon="el-icon-check" @click="enableData">启用</el-button>
            <el-button type="text" icon="el-icon-close" @click="disableData">禁用</el-button>
        </template>
        <template slot="query">
            <el-form ref="queryForm" :model="queryForm" label-position="left" label-width="150px">
                <el-row :gutter="40" type="flex">
                    <el-col :span="8">
                        <el-form-item label="用户名称" prop="UserName">
                            <el-input v-model="queryForm.UserName" clearable></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="用户账号" prop="UserAccount">
                            <el-input v-model="queryForm.UserAccount" clearable></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="用户角色" prop="UserRole">
                            <el-input v-model="queryForm.UserRole" clearable></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form>
        </template>
        <template slot="tabledata">
            <el-table ref="gridTable" v-bind:data="tableData" border stripe v-loading="loading" :height="$store.state.table_height" row-key="UserInfoId" @row-dblclick="dblClick">
                <el-table-column type="selection" width="35" :selectable="isSelectedTable">
                </el-table-column>
                <el-table-column label="操作" width="120px">
                    <template slot-scope="scope">
                        <el-button @click="editData(scope.row)" type="text" size="small">登录</el-button>
                        <el-button @click="roleData(scope.row)" type="text" size="small">配置角色</el-button>
                    </template>
                </el-table-column>
                <el-table-column prop="UserName" label="用户名称">
                </el-table-column>
                <el-table-column prop="UserAccount" label="用户账号">
                </el-table-column>
                <el-table-column prop="UserEmail" label="用户邮箱" show-overflow-tooltip>
                </el-table-column>
                <el-table-column label="用户角色"  show-overflow-tooltip>
                    <template slot-scope="scope">
                        <el-tag v-for="tag in scope.row.UserRoles" :key="tag.RoleInfoId" closable size="small" type="success" @close="handleTagClose(scope, tag.RoleInfoId)">
                            {{tag.RoleName}}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="LoginStatus" label="登录状态" width="100px">
                </el-table-column>
                <el-table-column prop="LoginOn" label="上次登录时间" width="140px">
                </el-table-column>
                <el-table-column prop="LoginClientIp" label="上次登录IP">
                </el-table-column>
                <el-table-column prop="UserStatus" label="启用状态" width="100px">
                </el-table-column>
            </el-table>
        </template>
    </wy-grid>
    <RoleDialog :visible.sync="roleVisible" v-if="roleVisible" @searchData="searchData" :rolesData="rolesData" :UserInfoId="userInfoId" />
</div>
</template>

<script>
import RoleDialog from './roledialog'
export default {
    data() {
        return {
            queryForm: {
                UserName: '',
                UserAccount: '',
                UserRole: '',
            },
            titles: ['系统管理', '用户管理', '用户列表信息'],
            tableData: [],
            loading: false,
            recordCount: 0,
            roleVisible: false,
            userInfoId: '',
            rolesData: [],
        }
    },
    props: {},
    components: {
        RoleDialog,
    },
    mounted: function () {
        this.searchData();
        this.loadRoleData();
    },
    computed: {
        searchList() {
            var searchList = [];
            if (this.queryForm.UserAccount) {
                searchList.push({
                    Type: 2,
                    Value: encodeURIComponent(this.queryForm.UserAccount.trim()),
                    Table: 'u.',
                    Key: 'Account'
                });
            }
            if (this.queryForm.UserName) {
                searchList.push({
                    Type: 2,
                    Value: this.queryForm.UserName.trim(),
                    Table: 'u.',
                    Key: 'Name'
                });
            }
            if (this.queryForm.UserRole) {
                searchList.push({
                    Type: 2,
                    Value: this.queryForm.UserRole.trim(),
                    Table: 'uir.',
                    Key: 'RoleName'
                });
            }
            return JSON.stringify(searchList);
        }
    },
    methods: {
        searchData: function () {
            var apiUrl = 'api/userinfo/list';
            var params = {
                searchList: this.searchList,
                pageIndex: this.$refs.grid.pageIndex,
                pageSize: this.$refs.grid.pageSize,
                orderBy: ' Order By UserName ASC'
            }
            this.loading = true;
            wy.get(apiUrl, params).then((res) => {
                this.tableData = res.RecordList;
                this.recordCount = res.RecordCount;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        reset: function () {
            this.$refs['queryForm'].resetFields();
        },
        dblClick: function (row) {
            this.$refs.gridTable.toggleRowSelection(row);
        },
        isSelectedTable: function (row) {
            return true;
        },
        enableData: function () {
            var that = this;
            wy.confirmClick(function () {
                var selection = that.$refs.gridTable.store.states.selection;
                if (selection == null || selection.length == 0) {
                    wy.showErrorMssg('请至少选择一条记录');
                    return;
                }
                var deleteIds = [];
                for (var i in selection) {
                    deleteIds.push(selection[i].UserInfoId);
                }
                var apiUrl = 'api/userinfo/enable';
                var params = {
                    ids: deleteIds
                };
                that.loading = true;
                wy.post(apiUrl, params).then((res) => {
                    that.searchData();
                    wy.showSuccessMssg(res);
                    that.loading = false;
                }).catch((error) => {
                    wy.showErrorMssg(error);
                    that.loading = false;
                });
            });
        },
        disableData: function () {
            var that = this;
            var selection = that.$refs.gridTable.store.states.selection;
            if (selection == null || selection.length == 0) {
                wy.showErrorMssg('请至少选择一条记录');
                return;
            }
            wy.confirmClick(function () {
                var deleteIds = [];
                for (var i in selection) {
                    deleteIds.push(selection[i].UserInfoId);
                }
                var apiUrl = 'api/userinfo/disable';
                var params = {
                    ids: deleteIds
                };
                that.loading = true;
                wy.post(apiUrl, params).then((res) => {
                    that.searchData();
                    wy.showSuccessMssg(res);
                    that.loading = false;
                }).catch((error) => {
                    wy.showErrorMssg(error);
                    that.loading = false;
                });
            });
        },
        roleData: function (row) {
            this.userInfoId = row.UserInfoId;
            this.roleVisible = true;
        },
        loadRoleData: function () {
            var apiUrl = 'api/roleinfo/roledata';
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.rolesData = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        handleTagClose: function (scope, roleId) {
            var apiUrl = 'api/userinfo/delrole';
            var params = {
                userId: scope.row.UserInfoId,
                roleId: roleId
            };
            wy.get(apiUrl, params).then((res) => {
                for (var i in scope.row.UserRoles) {
                    if (scope.row.UserRoles[i].RoleInfoId === roleId) {
                        scope.row.UserRoles.splice(i, 1);
                    }
                }
            }).catch((error) => {
                wy.showErrorMssg(error);
            });
        }
    }
}
</script>

<style>
</style>
