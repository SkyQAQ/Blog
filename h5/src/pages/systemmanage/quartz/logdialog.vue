<template>
<div id="quartzlog">
    <el-dialog title="运行日志" :visible.sync="visible" :before-close="close" width="60%">
        <div slot="title">
            <span>运行日志--[{{group}}][{{name}}]</span>
            <span style="font-size:14px;margin-right:5px;margin-left:20px;">只看异常</span>
            <el-switch v-model="onlyError" :disabled="loading" @change="loadData"></el-switch>
            <br>
            <el-button style="float:right" type="text" icon="el-icon-refresh" :disabled="loading" @click="loadData">刷新</el-button>
        </div>
        <el-table ref="gridTable" v-bind:data="tableData" border stripe v-loading="loading" height="442px" row-key="JobLogId">
            <el-table-column prop="StartTime" label="开始时间">
            </el-table-column>
            <el-table-column prop="EndTime" label="结束时间">
            </el-table-column>
            <el-table-column prop="Host" label="运行主机">
            </el-table-column>
            <el-table-column prop="Status" label="运行结果">
                <template slot-scope="scope">
                    <el-popover placement="right-start" width="300" trigger="hover" :content="scope.row.Result">
                        <el-tag type="success" slot="reference" size="small" v-if="scope.row.Status==='成功'">{{scope.row.Status}}</el-tag>
                        <el-tag type="danger" slot="reference" size="small" v-else>{{scope.row.Status}}</el-tag>
                    </el-popover>
                </template>
            </el-table-column>
        </el-table>
        <el-pagination @size-change="handleSizeChange" @current-change="handleIndexChange" :current-page="pageIndex" :page-size="pageSize" :page-sizes="pageSizes" layout="total, sizes, prev, pager, next, jumper" :total="recordCount">
        </el-pagination>
        <br>
    </el-dialog>
</div>
</template>

<script>
export default {
    data() {
        return {
            tableData: [],
            recordCount: 0,
            pageSizes: [10, 20, 30, 40, 50],
            pageSize: 10,
            pageIndex: 1,
            onlyError: false,
            loading: false,
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
            require: true
        },
        group: {
            type: String,
            default: '',
            require: false
        },
        name: {
            type: String,
            default: '',
            require: false
        },
    },
    computed: {

    },
    mounted: function () {
        if (this.group && this.name) {
            this.loadData();
        }
    },
    methods: {
        loadData: function () {
            var apiUrl = 'api/quartz/loginfo';
            var params = {
                group: this.group,
                name: this.name,
                pageIndex: this.pageIndex,
                pageSize: this.pageSize,
                onlyError: this.onlyError
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
        handleSizeChange: function (val) {
            this.pageSize = val;
            this.loadData();
        },
        handleIndexChange: function (val) {
            this.pageIndex = val;
            this.loadData();
        },
        close: function () {
            this.loading = false;
            this.$emit('update:visible', false);
        }
    }
}
</script>
