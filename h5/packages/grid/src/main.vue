<template>
    <div id="wy_grid" class="wy-grid">
        <el-card :body-style="{ padding: '5px 20px' }">
            <div class="wy-grid-header">
                <slot name="button"></slot>
                <el-button v-if="$slots.query" style="float:right;padding: 5px 30px 5px 10px" type="primary" @click="hideClick" :icon="IsQueryShow?'el-icon-arrow-down':'el-icon-arrow-up'">{{IsQueryShow?'  收起':'  展开'}}</el-button>
            </div>
            <div id="wy_grid_query" class="wy-grid-query" v-show="IsQueryShow&&$slots.query" @keyup.enter.prevent="handleSearch">
                <div class="wy-grid-lessquery" >
                    <slot name="query"></slot>
                </div>
                <div id="wy_grid_morequery" class="wy-grid-morequery" v-show="IsMoreQueryShow">
                    <slot name="morequery" v-if="IsMoreQueryShow"></slot>
                </div>
                <el-row :gutter="40" type="flex" v-if="$slots.query">
                    <el-col v-bind:span="24" style="float:right;text-align:right;">
                        <el-button style="padding: 5px 30px 5px 10px" type="primary" v-if="$slots.morequery" @click="moreClick" :icon="IsMoreQueryShow?'el-icon-arrow-down':'el-icon-arrow-up'">更多条件</el-button>
                        <el-button @click="handleReset" style="padding: 5px 30px 5px 30px">重置</el-button>
                        <el-button @click="handleSearch" style="padding: 5px 30px 5px 30px;margin-left:10px;" type="primary">查询</el-button>
                    </el-col>
                </el-row>
            </div>
            <div class="wy-grid-body">
                <slot name="tabledata"></slot>
            </div>
            <div class="wy-grid-fotter">
                <el-pagination v-if="showPagination" @size-change="handleSizeChange" @current-change="handleIndexChange" :current-page="pageIndex" :page-size="pageSize" :page-sizes="pageSizes" :layout="layout" :total="recordCount">
                </el-pagination>
                <div v-else style="height:32px">
                </div>
            </div>
        </el-card>
    </div>
</template>

<script>
export default {
    name: "WyGrid",
    props: {
        showPagination: {
            type: Boolean,
            required: false,
            default: true
        },
        recordCount: {
            type: Number,
            default: 0,
            required: false
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
        loading: {
            type: Boolean,
            required: false,
            default: false,
        }
    },
    data() {
        return{
            IsQueryShow: true,
            IsMoreQueryShow: false,
            pageIndex: 1,
            pageSize: 20,
        }
    },
    created() {
        
    },
    mounted() {
        this.handleCollapse();
        window.onresize = () => {
            this.handleCollapse();
        };
    },
    methods: {
        hideClick: function(){
            this.IsQueryShow = !this.IsQueryShow;
            this.handleCollapse();
        },
        moreClick: function(){
            this.IsMoreQueryShow = !this.IsMoreQueryShow;
            this.handleCollapse();
        },
        handleCollapse: function(){
            let that = this;
            this.$nextTick(() => {
                var divHeight = document.getElementById('wy_grid_query').clientHeight;
                if(divHeight === 0){
                    divHeight = divHeight - 1;
                }
                var tableHeight = document.body.clientHeight - this.$store.state.top_height;
                that.$store.commit('update_table_height', tableHeight - divHeight);
            });
        },
        handleSizeChange: function(val){
            this.pageSize = val;
            this.$emit('handleSearch');
        },
        handleIndexChange: function(val){
            this.pageIndex = val;
            this.$emit('handleSearch');
        },
        handleSearch: function(){
            this.pageIndex = 1;
            this.$emit('handleSearch');
        },
        handleReset: function(){
            this.$emit('handleReset');
        }
    }
}
</script>
<style>
</style>
