<template>
   <div id="wy_header" class="wy-header">
       <el-card :body-style="{ padding: '10px' }">
            <el-breadcrumb separator-class="el-icon-arrow-right" >
                <el-breadcrumb-item v-for="title in titles" :key="title">{{title}}</el-breadcrumb-item>
                <div class="wy-header-button"> 
                    <slot></slot>
                    <div class="wy-divider-y" v-if="showBack || showAutoback"></div>
                    <el-button type="text" @click="handleBackClick" v-if="showBack || showAutoback">返回</el-button>
                </div>
            </el-breadcrumb>
       </el-card>
   </div> 
</template>

<script>
    export default {
        name: 'WyHeader',
        props: {
            titles: {
                type: Array,
                required: true
            },
            showBack: {
                type: Boolean,
                default: false,
                required: false
            },
            showAutoback: {
                type: Boolean,
                default: false,
                required: false
            }
        },
        methods: {
            handleBackClick() {
                if (this.showAutoback) {
                    if(!this.$route.params.id){
                        this.$confirm('当前单据为保存，是否确认返回？', '提示', {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            type: 'warning'
                        }).then(() => {
                            this.$router.back();
                        }).catch(() => {});
                    }else{
                        this.$router.back();
                    }
                } else {
                    this.$emit("goback");
                }
            }
        }
    }
</script>
<style>
</style>
