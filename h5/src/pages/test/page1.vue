<template>
<div id="testpage1">
    <wy-header :titles="titles">
    </wy-header>
    <wy-content>
        <el-button @click="play">播放</el-button>
    </wy-content>
    <VedioPlayerDialog ref="vedio" />
</div>
</template>

<script>
import VedioPlayerDialog from './vedioplayer'

export default {
    data() {
        return {
            titles: ['阳哥', '妈的还有谁'],
            urlPrefix: '',
        }
    },
    props: {

    },
    components: {
        VedioPlayerDialog
    },
    mounted: function () {
        var comfig = JSON.parse(localStorage.getItem(_const.Key_CommonConfig)) || { Url: ''};
        this.urlPrefix = comfig.Url === '' ? wy.getBaseUrl() : comfig.Url;
    },
    computed: {

    },
    methods: {
        play: function () {
            var src = this.urlPrefix + 'vedio/test.mp4';
            this.$refs.vedio.open('video/mp4', src);
        }
    }
}
</script>

<style>
</style>
