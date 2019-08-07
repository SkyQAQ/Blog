<template>
<div id="attach_upload" class="attach-upload">
    <el-dialog title="附件上传" :visible.sync="visible" v-if="visible" :before-close="close" :close-on-click-modal="false" width="30%">
        <el-upload class="upload-file" ref="upload" :action="uploadUrl" :headers="headers" :data="params" :accept="limit.Type" :on-exceed="onExceed" :on-preview="handlePreview" :before-remove="handleRemove" :before-upload="beforeUpload" :on-success="onSuccess" :on-error="onError" :on-change="onChange" :file-list="fileList" :limit="limit.MaxCount" :auto-upload="false" :multiple="true">
            <el-button slot="trigger" size="small" :loading="loading" type="primary">选取文件</el-button>
            <el-button style="margin-left: 10px;" :loading="loading" size="small" type="success" @click="submitUpload">上传到服务器</el-button>
            <div slot="tip" class="el-upload__tip">只能上传 {{limit.Type}} 文件，且不超过 {{limit.MaxLength}} M</div>
        </el-upload>
        <span slot="footer">
            <div class="upload-progress">
                <pre>{{uploadResult}}</pre>
            </div>
        </span>
    </el-dialog>
</div>
</template>

<script>
export default {
    data() {
        return {
            visible: false,
            loading: false,
            fileList: [],
            uploadUrl: '',
            headers: {},
            params: {},
            limit: {
                Type: '',
                MaxCount: 0,
                MaxLength: 0,
            },
            uploadResult: '',
        }
    },
    props: {},
    computed: {},
    mounted: function () {},
    methods: {
        show: function (moduleType, moduleId) {
            this.load(moduleType, moduleId);
            this.visible = true;
        },
        load: function (moduleType, moduleId) {
            this.params = {
                ModuleType: moduleType,
                ModuleId: moduleId,
                FileDir: '',
            }
            this.loadHeader();
            this.limit = JSON.parse(localStorage.getItem(_const.Key_UploadLimit)) || { Url: ''};
            this.uploadUrl = this.limit.Url === '' ? wy.getBaseUrl() + 'api/attachment/upload' : this.limit.Url + 'api/attachment/upload';
        },
        loadHeader: function () {
            var auth = this.$store.state.auth;
            this.headers = {
                'Authorization': auth.token_type + ' ' + auth.access_token,
            };
        },
        reset: function () {
            this.fileList = [];
            this.headers = {};
            this.params = {};
            this.limit = {};
            this.uploadResult = '';
        },
        submitUpload: function () {
            this.uploadResult = '';
            this.$refs.upload.submit();
        },
        handleRemove: function (file, fileList) {
            var that = this;
            if (!file.response || !file.response.data) {
                return true;
            }
            wy.confirmClick(function () {
                var deleteIds = [file.response.data];
                var apiUrl = 'api/attachment/delete';
                var params = {
                    ids: deleteIds
                };
                wy.post(apiUrl, params).then((res) => {
                    for (var i in fileList) {
                        if (fileList[i] == file) {
                            fileList.splice(i, 1);
                        }
                    }
                    wy.showSuccessMssg(res);
                }).catch((error) => {
                    wy.showErrorMssg(error);
                });
            });
            return false;
        },
        handlePreview: function (file) {
            console.log(file);
        },
        beforeUpload: function (file) {},
        onChange: function (file, fileList) {
            if (file.size > this.limit.MaxLength * 1024 * 1024) {
                this.uploadResult += file.name + ':文件大小超过限制 \n';
                for (var i in fileList) {
                    if (fileList[i] == file) {
                        fileList.splice(i, 1);
                    }
                }
            }
        },
        onSuccess: function (response, file, fileList) {},
        onError: function (error, file, fileList) {
            this.uploadResult = JSON.parse(error.message).mssg;
        },
        onExceed: function (files, fileList) {
            this.uploadResult = '每次最多上传' + this.limit.MaxCount + '个文件';
        },
        close: function () {
            this.$emit('searchData');
            this.reset();
            this.$refs.upload.clearFiles();
            this.visible = false;
        }
    }
}
</script>

<style scoped>
.upload-file {
    height: 210px;
}

.upload-progress {
    text-align: left;
    margin-top: 5px;
}

.upload-progress pre {
    font-size: 12px;
    color: red;
}
</style>
