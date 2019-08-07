<template>
<div id="useravataredit">
    <avatar-upload ref="uploadAvatar" field="avatar" v-if="visible" @before-close="close" @crop-success="cropSuccess" @crop-upload-success="cropUploadSuccess" @crop-upload-fail="cropUploadFail" v-model="visible" :width="300" :height="300" :url="uploadUrl" :params="params" :headers="headers" img-format="png"></avatar-upload>
</div>
</template>

<script>
import avatarUpload from 'vue-image-crop-upload';

export default {
    data() {
        return {
            loading: false,
            visible: false,
            params: {}, //其他参数
            uploadUrl: '',
            headers: {},
            imgData: '',
        }
    },
    props: {},
    computed: {},
    components: {
        avatarUpload
    },
    mounted: function () {
        
    },
    methods: {
        show: function (uploadUrl) {
            var auth = this.$store.state.auth;
            this.uploadUrl = uploadUrl;
            this.headers = {
                'Authorization': auth.token_type + ' ' + auth.access_token,
            };
            this.visible = true;
        },
        close: function () {
            this.uploadUrl = '';
            this.headers = {};
            this.visible = false;
        },
        /**
         * crop success
         *
         * [param] imgDataUrl
         * [param] field
         */
        cropSuccess(imgDataUrl, field) {
            this.imgData = imgDataUrl;
        },
        /**
         * upload success
         *
         * [param] jsonData   服务器返回数据，已进行json转码
         * [param] field
         */
        cropUploadSuccess(jsonData, field) {
            if(jsonData.code === 1){
                this.$emit('loadAvatar', this.imgData);
                this.$store.commit('update_user_avatar', jsonData.data);
            }
            else{
                wy.showErrorMssg(jsonData.mssg);
            }
        },
        /**
         * upload fail
         *
         * [param] status    server api return error status, like 500
         * [param] field
         */
        cropUploadFail(status, field) {
        }
    }
}
</script>
