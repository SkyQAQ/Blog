<template>
<div id="dreaminfolist">
    <wy-header :titles="titles"></wy-header>
    <wy-grid ref="grid" @handleSearch="searchData" @handleReset="reset" :recordCount="recordCount" :loading="loading">
        <template slot="button">
            <el-button type="text" icon="el-icon-plus" @click="createData">新建</el-button>
            <el-button type="text" icon="el-icon-delete" @click="deleteData">删除</el-button>
            <el-button type="text" icon="el-icon-download" @click="exportData">导出</el-button>
        </template>
        <template slot="query">
            <el-form ref="queryForm" :model="queryForm" label-position="left" label-width="150px">
                <el-row :gutter="40" type="flex">
                    <el-col :span="8">
                        <el-form-item label="彩票类型" prop="Type">
                            <el-select placeholder="请选择" v-model="queryForm.Type">
                                <el-option v-for="item in DreamType" :key="item.Value" :label="item.Text" :value="item.Value">
                                </el-option>
                            </el-select>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="起始期数" prop="StartStage">
                            <el-input v-model="queryForm.StartStage" type="number" clearable></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="截止期数" prop="EndStage">
                            <el-input v-model="queryForm.EndStage" type="number" clearable></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form>
        </template>
        <template slot="morequery">
            <el-form ref="queryForm" :model="queryForm" label-position="left" label-width="150px">
                <el-row :gutter="40" type="flex">
                    <el-col :span="8">
                        <el-form-item label="梦想编码" prop="DreamCode">
                            <el-input v-model="queryForm.DreamCode" clearable></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form>
        </template>
        <template slot="tabledata">
            <el-table ref="gridTable" v-bind:data="tableData" border stripe v-loading="loading" :height="$store.state.table_height" row-key="DreamInfoId" @row-dblclick="dblClick">
                <el-table-column type="selection" width="35" :selectable="isSelectedTable">
                </el-table-column>
                <el-table-column label="操作" width="120px">
                    <template slot-scope="scope">
                        <el-button @click="editData(scope.row)" type="text" size="small">编辑</el-button>
                    </template>
                </el-table-column>
                <el-table-column prop="TypeText" label="彩票类型">
                </el-table-column>
                <el-table-column prop="DreamCode" label="梦想名称">
                </el-table-column>
                <el-table-column prop="StartStage" label="起始期数">
                </el-table-column>
                <el-table-column prop="EndStage" label="截止期数">
                </el-table-column>
                <el-table-column prop="CreatedOn" label="创建时间">
                </el-table-column>
                <el-table-column prop="ModifiedOn" label="修改时间">
                </el-table-column>
            </el-table>
        </template>
    </wy-grid>
    <EditDialog :visible.sync="editVisible" v-if="editVisible" @searchData="searchData" :DreamInfoId="dreamInfoId" />
</div>
</template>

<script>
import EditDialog from './editdialog'
export default {
    data() {
        return {
            queryForm: {
                DreamCode: '',
                Type: '',
                StartStage: '',
                EndStage: '',
            },
            titles: ['系统管理', '梦想管理', '梦想列表信息'],
            tableData: [],
            loading: false,
            DreamType: [{
                    Text: '大乐透',
                    Value: 1
                },
                {
                    Text: '排列三',
                    Value: 2
                },
                {
                    Text: '排列五',
                    Value: 3
                },
            ],
            recordCount: 0,
            editVisible: false,
            menuVisible: false,
            dreamInfoId: '',
        }
    },
    props: {},
    components: {
        EditDialog,
    },
    mounted: function () {
        this.searchData();
    },
    computed: {
        searchList() {
            var searchList = [];
            if (this.queryForm.DreamCode) {
                searchList.push({
                    Type: 2,
                    Value: encodeURIComponent(this.queryForm.DreamCode),
                    Table: '',
                    Key: 'DreamCode'
                });
            }
            if (this.queryForm.Type) {
                searchList.push({
                    Type: 1,
                    Value: this.queryForm.Type,
                    Table: '',
                    Key: 'Type'
                });
            }
            if (this.queryForm.StartStage) {
                searchList.push({
                    Type: 4,
                    Value: this.queryForm.StartStage,
                    Table: '',
                    Key: 'beforeStartStage'
                });
            } 
            if (this.queryForm.EndStage) {
                searchList.push({
                    Type: 4,
                    Value: this.queryForm.EndStage,
                    Table: '',
                    Key: 'beforeEndStage'
                });
            }
            return JSON.stringify(searchList);
        }
    },
    methods: {
        searchData: function () {
            var apiUrl = 'api/dreaminfo/list';
            var params = {
                searchList: this.searchList,
                pageIndex: this.$refs.grid.pageIndex,
                pageSize: this.$refs.grid.pageSize,
                orderBy: ' Order By StartStage ASC'
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
            this.dreamInfoId = '';
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
                    deleteIds.push(selection[i].DreamInfoId);
                }
                var apiUrl = 'api/dreaminfo/delete';
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
            var apiUrl = 'api/dreaminfo/list';
            var params = {
                searchList: this.searchList,
                pageIndex: -1,
                pageSize: -1,
                orderBy: ' Order By RoleName ASC'
            }
            this.loading = true;
            wy.get(apiUrl, params).then((res) => {
                wy.downloadExlByBlob(res.ExportBuffString, '梦想信息');
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        editData: function (row) {
            this.dreamInfoId = row.DreamInfoId;
            this.editVisible = true;
        },
    }
}
</script>

<style>
</style>
