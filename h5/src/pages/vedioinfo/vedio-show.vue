<template>
<div id="vedio_show">
    <div id="vedio_show_body">
        <el-row :gutter="10">
            <el-col :span="6" v-for="o in tableData" :key="o.VedioInfoId" >
                <el-card :body-style="{ padding: '5px' }">
                    <img width="80%" height="80%" :src=" urlPrefix + 'vedio/' + o.Poster" class="image">
                    <div style="padding: 14px;">
                        <span>{{ o.Description }}</span>
                        <div class="bottom clearfix">
                            <time class="time">{{ o.CreatedOn }}</time>
                            <el-button type="text" class="button" @click="play(o)">播放</el-button>
                        </div>
                    </div>
                </el-card>
            </el-col>
        </el-row>
    </div>
    <div id="vedio_show_fotter">
        <el-pagination @size-change="handleSizeChange" @current-change="handleIndexChange" :current-page="pageIndex" :page-size="pageSize" :page-sizes="pageSizes" :layout="layout" :total="recordCount">
        </el-pagination>
    </div>
    <VedioPlayerDialog ref="vedio" />
</div>
</template>

<script>
import VedioPlayerDialog from './vedio-player'

export default {
    name: 'VedioShow',
    data() {
        return {
            pageIndex: 1,
            pageSize: 20,
            recordCount: 0,
            tableData: {},
            loading: false,
        }
    },
    props: {
        type: {
            type: Number,
            required: true,
        },
        pageSizes: {
            type: Array,
            required: false,
            default: function(){
                return [10, 20, 30, 40, 50];
            }
        },
        layout: {
            type: String,
            required: false,
            default: 'total, sizes, prev, pager, next, jumper'
        },
        urlPrefix: {
            type: String,
            required: true
        }
    },
    components: {
        VedioPlayerDialog
    },
    mounted: function () {
    },
    computed: {

    },
    methods: {
        searchData: function () {
            var apiUrl = 'api/vedioinfo/list';
            var params = {
                searchList: this.searchList,
                pageIndex: this.pageIndex,
                pageSize: this.pageSize,
                orderBy: ' Order By CreatedOn DESC',
                type: this.type
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
         handleSizeChange: function(val){
            this.pageSize = val;
            this.searchData();
        },
        handleIndexChange: function(val){
            this.pageIndex = val;
            this.searchData();
        },
        play: function(obj){
            this.$refs.vedio.open(obj.SourceType, this.urlPrefix + 'vedio/' + obj.SourceUrl, this.urlPrefix + 'vedio/' + obj.Poster);
        }
    }
}
</script>

<style scoped>
    #vedio_show_body {
        overflow-y: auto;
        overflow-x: hidden; 
        height: 78%;
    }
    .time {
        font-size: 13px;
        color: #999;
    }
    
    .bottom {
        margin-top: 13px;
        line-height: 12px;
    }

    .button {
        padding: 0;
        float: right;
    }

    .image {
        width: 100%;
        display: block;
    }

    .clearfix:before,
    .clearfix:after {
        display: table;
        content: "";
    }
  
    .clearfix:after {
        clear: both
    }
</style>
