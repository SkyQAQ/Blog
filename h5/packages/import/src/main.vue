<template>
    <el-dialog title="数据导入" :visible.sync="visible" :close-on-click-modal="false" :before-close="close" width="400px">
        <div id="import" class="import">
            <span style="color:red;font-size:12px;">
                只能上传Excel(xls或xlsx)文件，且记录数不超过5000条
            </span>
            <el-upload
                ref="upload"
                class="upload-demo"
                drag
                action='/'
                :multiple="false"
                :on-change="handleChange"
                :show-file-list="false"
                :auto-upload="false"
                :http-request="handleHttpRequest"
                accept=".xls,.xlsx">
                <i class="el-icon-upload"></i>
                <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                <div class="el-upload__tip" slot="tip">已选择文件：{{selectedFile.name}}</div>
            </el-upload>
            <div class="div-import-result">
                <pre>{{importResult}}</pre>
            </div>
        </div> 
        <span slot="footer">
            <el-button @click="handleDownload" :loading="loading" v-if="templateUrl">模板下载</el-button>
            <el-button @click="handleImport" type="primary" :loading="loading">导入</el-button>
        </span>
    </el-dialog>
</template>

<script>
    export default {
        name: 'WyImport',
        data() {
            return {
                loading: false,
                selectedFile: {},
                isImporting: false,
                isSuccessed: false,
                importResult: '',
            }
        },
        props: {
            visible: {
                type: Boolean,
                default: false,
                require: true
            },
            actionApi: {
                type: String,
                require: true
            },
            onSuccess: Function,
            template: {
                type: String,
                default: '',
                require: false
            },
        },
        computed: {
            templateUrl(){
                return wy.getBaseUrl() + 'template/' + this.template;
            }
        },
        mounted: function() {
            if(this.RoleInfoId){
                this.load();
            }
        },
        methods: {
            handleHttpRequest: function(){
                if (this.isImporting) {
                    return;
                }
                this.isImporting = true;
                this.loading = true;
                var data = new FormData();
                data.append("file", this.selectedFile.raw);
                wy.post(this.actionApi, data).then((res)=>{
                    this.importResult = res;
                    this.isImporting = false;
                    this.isSuccessed = true;
                    if (this.onSuccess) {
                        this.onSuccess();
                    }
                    this.loading = false;
                }).catch((error) =>{
                    this.importResult = error;
                    this.isImporting = false;
                    this.isSuccessed = false;
                    this.loading=false;
                });
            },
            handleChange(file, fileList) {
                this.loading = true;
                this.selectedFile = file;
                this.loading = false;
            },
            handleImport: function(){
                if(this.$refs.upload.uploadFiles.length === 0){
                    wy.showErrorMssg('请选择待上传的文件！');
                    return
                }
                if (this.isImporting) {
                    return;
                }
                this.$refs.upload.submit();
            },
            handleDownload: function(){
                console.log(this.templateUrl)
                window.open(this.templateUrl);
            },
            close: function(){
                this.$refs.upload.clearFiles();
                this.loading = false;
                this.$emit('update:visible', false);
            }
        }
    }
</script>

