<template>
<div id="vedioinfolist">
    <wy-header :titles="titles">
    </wy-header>
    <wy-content>
        <el-tabs v-model="activeName" @tab-click="handleClick">
            <el-tab-pane label="所有视频" name="all">
                <vedio-show ref="vs_all" :urlPrefix="urlPrefix" :type='1'></vedio-show>
            </el-tab-pane>
            <el-tab-pane label="最新视频" name="latest">最新视频</el-tab-pane>
            <el-tab-pane label="最热视频" name="hotest">最热视频</el-tab-pane>
            <el-tab-pane name="my">
                <span slot="label">
                    我的视频 | 
                    <el-button type="text" icon="el-icon-upload2" @click="upload_visible=true">上传</el-button>
                </span>
                <vedio-show ref="vs_my" :urlPrefix="urlPrefix" :type='4'></vedio-show>
                <vedio-upload :visible="upload_visible" :urlPrefix="urlPrefix" @close="close"></vedio-upload>
            </el-tab-pane>
        </el-tabs>
    </wy-content>
</div>
</template>

<script>
import VedioShow from './vedio-show'
import VedioUpload from './vedio-upload'

export default {
    data() {
        return {
            titles: ['Vedio log', '记录美好瞬间'],
            urlPrefix: '',
            activeName: 'all',
            vediodata: {},
            upload_visible: false,
        }
    },
    props: {

    },
    components: {
        VedioShow,
        VedioUpload
    },
    mounted: function () {
        var comfig = JSON.parse(localStorage.getItem(_const.Key_CommonConfig)) || { Url: ''};
        this.urlPrefix = comfig.Url === '' ? wy.getBaseUrl() : comfig.Url;
        this.handleClick({ name: this.activeName });
    },
    computed: {

    },
    methods: {
        handleClick(tab, event) {
            if(tab.name == 'all'){
                this.$refs.vs_all.searchData();
            }
            else if(tab.name == 'latest'){
                wy.showSuccessMssg('上线ing......')
            }
            else if(tab.name == 'hotest'){
                wy.showSuccessMssg('上线ing......')
            }
            else if(tab.name == 'my'){
                this.$refs.vs_my.searchData();
            }
        },
        close(){
            this.upload_visible = false;
            this.$refs.vs_my.searchData();
        }
    }
}
</script>

<style>
</style>
