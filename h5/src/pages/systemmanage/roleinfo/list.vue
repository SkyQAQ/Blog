<template>
<div id="roleinfolist">
    <wy-header :titles="titles"></wy-header>
    <wy-grid ref="grid" @handleSearch="searchData" @handleReset="reset" :recordCount="recordCount" :loading="loading">
        <template slot="button">
            <el-button type="text" icon="el-icon-plus" @click="createData">新建</el-button>
            <el-button type="text" icon="el-icon-delete" @click="deleteData">删除</el-button>
            <el-button type="text" icon="el-icon-download" @click="exportData">导出</el-button>
            <el-button type="text" icon="el-icon-upload2" @click="importData">导入</el-button>
        </template>
        <template slot="query">
            <el-form ref="queryForm" :model="queryForm" label-position="left" label-width="150px">
                <el-row :gutter="40" type="flex">
                    <el-col :span="8">
                        <el-form-item label="角色名称" prop="RoleName">
                            <el-input v-model="queryForm.RoleName" clearable></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="角色编码" prop="RoleCode">
                            <el-input v-model="queryForm.RoleCode" clearable></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form>
        </template>
        <template slot="morequery">
            <el-form ref="queryForm" :model="queryForm" label-position="left" label-width="150px">
                <el-row :gutter="40" type="flex">
                    <el-col :span="8">
                        <el-form-item label="日期类型" prop="DateType">
                            <el-select placeholder="请选择" v-model="queryForm.DateType">
                                <el-option v-for="item in DateType" :key="item.Value" :label="item.Text" :value="item.Value">
                                </el-option>
                            </el-select>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="从" prop="DateFrom">
                            <el-date-picker v-model="queryForm.DateFrom" type="date" placeholder="请选择" clearable :disabled="!queryForm.DateType" :editable="false"></el-date-picker>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="到" prop="DateTo">
                            <el-date-picker v-model="queryForm.DateTo" type="date" placeholder="请选择" clearable :disabled="!queryForm.DateType" :editable="false"></el-date-picker>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form>
        </template>
        <template slot="tabledata">
            <el-table ref="gridTable" v-bind:data="tableData" border stripe v-loading="loading" :height="$store.state.table_height" row-key="RoleInfoId" @row-dblclick="dblClick">
                <el-table-column type="selection" width="35" :selectable="isSelectedTable">
                </el-table-column>
                <el-table-column label="操作" width="120px">
                    <template slot-scope="scope">
                        <el-button @click="editData(scope.row)" type="text" size="small">编辑</el-button>
                        <el-button @click="menuData(scope.row)" type="text" size="small">配置菜单</el-button>
                    </template>
                </el-table-column>
                <el-table-column prop="RoleCode" label="角色编码">
                </el-table-column>
                <el-table-column prop="RoleName" label="角色名称">
                </el-table-column>
                <el-table-column prop="CreatedBy" label="创建人">
                </el-table-column>
                <el-table-column prop="ModifiedBy" label="修改人">
                </el-table-column>
                <el-table-column prop="CreatedOn" label="创建时间">
                </el-table-column>
                <el-table-column prop="ModifiedOn" label="修改时间">
                </el-table-column>
            </el-table>
        </template>
    </wy-grid>
    <EditDialog :visible.sync="editVisible" v-if="editVisible" @searchData="searchData" :RoleInfoId="roleInfoId" :menutreedata="menutreedata" />
    <MenuDialog :visible.sync="menuVisible" v-if="menuVisible" :RoleInfoId="roleInfoId" :menutreedata="menutreedata" />
    <wy-import :visible.sync='uploadVisible' v-if="uploadVisible" actionApi='api/roleinfo/importrole' template='角色导入模板.xlsx' :onSuccess='searchData'></wy-import>
</div>
</template>

<script>
import EditDialog from './editdialog'
import MenuDialog from './menudialog'
export default {
    data() {
        return {
            queryForm: {
                RoleName: '',
                RoleCode: '',
                DateFrom: '',
                DateTo: '',
                DateType: 'create',
            },
            titles: ['系统管理', '角色管理', '角色列表信息'],
            tableData: [],
            loading: false,
            DateType: [{
                    Text: '创建时间',
                    Value: 'create'
                },
                {
                    Text: '修改时间',
                    Value: 'modify'
                }
            ],
            recordCount: 0,
            editVisible: false,
            menuVisible: false,
            roleInfoId: '',
            menutreedata: [],
            uploadVisible: false,
        }
    },
    props: {},
    components: {
        EditDialog,
        MenuDialog,
    },
    mounted: function () {
        this.searchData();
        this.loadMenuTreeData();
    },
    computed: {
        searchList() {
            var searchList = [];
            if (this.queryForm.RoleName) {
                searchList.push({
                    Type: 2,
                    Value: encodeURIComponent(this.queryForm.RoleName.trim()),
                    Table: '',
                    Key: 'RoleName'
                });
            }
            if (this.queryForm.RoleCode) {
                searchList.push({
                    Type: 2,
                    Value: this.queryForm.RoleName.trim(),
                    Table: '',
                    Key: 'RoleCode'
                });
            }
            if (this.queryForm.DateFrom) {
                if (this.queryForm.DateType == "create") {
                    searchList.push({
                        Type: 4,
                        Value: wy.formatDateTime(this.queryForm.DateFrom, "yyyy-mm-dd"),
                        Table: "",
                        Key: "afterCreatedOn"
                    });
                } else if (this.queryForm.DateType == "modify") {
                    searchList.push({
                        Type: 4,
                        Value: wy.formatDateTime(this.queryForm.DateFrom, "yyyy-mm-dd"),
                        Table: "",
                        Key: "afterModifiedOn"
                    });
                }
            }
            if (this.queryForm.DateTo) {
                if (this.queryForm.DateType == "create") {
                    searchList.push({
                        Type: 4,
                        Value: wy.formatDateTime(this.queryForm.DateTo, "yyyy-mm-dd"),
                        Table: "",
                        Key: "beforeCreatedOn"
                    });
                } else if (this.queryForm.DateType == "modify") {
                    searchList.push({
                        Type: 4,
                        Value: wy.formatDateTime(this.queryForm.DateTo, "yyyy-mm-dd"),
                        Table: "",
                        Key: "beforeModifiedOn"
                    });
                }
            }
            return JSON.stringify(searchList);
        }
    },
    methods: {
        searchData: function () {
            var apiUrl = 'api/roleinfo/list';
            var params = {
                searchList: this.searchList,
                pageIndex: this.$refs.grid.pageIndex,
                pageSize: this.$refs.grid.pageSize,
                orderBy: ' Order By RoleName ASC'
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
        createData: function () {
            this.roleInfoId = '';
            this.editVisible = true;
        },
        deleteData: function () {
            var that = this;
            var selection = that.$refs.gridTable.store.states.selection;
            if (selection == null || selection.length == 0) {
                wy.showErrorMssg('请至少选择一条记录');
                return;
            }
            wy.confirmClick(function () {
                var deleteIds = [];
                for (var i in selection) {
                    deleteIds.push(selection[i].RoleInfoId);
                }
                var apiUrl = 'api/roleinfo/delete';
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
        exportData: function () {
            var apiUrl = 'api/roleinfo/list';
            var params = {
                searchList: this.searchList,
                pageIndex: -1,
                pageSize: -1,
                orderBy: ' Order By RoleName ASC'
            }
            this.loading = true;
            wy.get(apiUrl, params).then((res) => {
                wy.downloadExlByBlob(res.ExportBuffString, '角色信息');
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        editData: function (row) {
            this.roleInfoId = row.RoleInfoId;
            this.editVisible = true;
        },
        importData: function (row) {
            this.uploadVisible = true;
        },
        menuData: function (row) {
            this.roleInfoId = row.RoleInfoId;
            this.menuVisible = true;
        },
        loadMenuTreeData: function () {
            var apiUrl = 'api/menuinfo/treedata';
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.menutreedata = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        }
    }
}
</script>

<style>
</style>
