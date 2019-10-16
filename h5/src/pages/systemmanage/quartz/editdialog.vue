<template>
<div id="quartzedit">
    <el-dialog title="任务信息" :visible.sync="visible" :before-close="close" :close-on-click-modal="false" width="30%">
        <el-form ref="editForm" :rules="rules" :model="model" label-width="100px">
            <el-form-item label="任务分组" prop="JobGroup">
                <el-input type="text" v-model="model.JobGroup" :disabled="JobId!==''" auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="任务名称" prop="JobName">
                <el-input type="text" v-model="model.JobName" :disabled="JobId!==''" auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="任务描述" prop="JobDesc">
                <el-input type="text" v-model="model.JobDesc" auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="任务类名" prop="JobClass">
                <wy-select v-model="model.JobClass" :disabled="JobId!==''" apiUrl="api/quartz/jobs"></wy-select>
            </el-form-item>
            <el-form-item label="Cron表达式" prop="Cron">
                <el-input type="text" v-model="model.Cron" auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="Cron描述" prop="CronDesc">
                <el-input type="text" v-model="model.CronDesc" auto-complete="off">
                </el-input>
            </el-form-item>
        </el-form>
        <span slot="footer">
            <el-button @click="save(create)" :loading="loading" style="width:120px" v-show="!this.JobId">保存并新建</el-button>
            <el-button @click="close" style="width:120px" :loading="loading" v-show="this.JobId">关闭</el-button>
            <el-button @click="save(close)" type="primary" :loading="loading" style="width:120px">保存</el-button>
        </span>
    </el-dialog>
</div>
</template>

<script>
export default {
    data() {
        return {
            model: {
                JobInfoId: '',
                JobName: '',
                JobGroup: '',
                JobDesc: '',
                Cron: '',
                CronDesc: '',
                JobClass: '',
            },
            loading: false,
            rules: {
                JobGroup: [{
                    required: true,
                    message: '请输入任务分组',
                    trigger: 'blur'
                }],
                JobName: [{
                    required: true,
                    message: '请输入任务名称',
                    trigger: 'blur'
                }, ],
                JobClass: [{
                    required: true,
                    message: '请选择任务类名',
                    trigger: 'change'
                }],
                Cron: [{
                    required: true,
                    message: '请输入Cron表达式',
                    trigger: 'blur'
                }, ]
            },
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
            require: true
        },
        JobId: {
            type: String,
            default: '',
            require: false
        },
    },
    computed: {

    },
    mounted: function () {
        if (this.JobId) {
            this.load();
        }
    },
    methods: {
        load: function () {
            var apiUrl = 'api/quartz/edit?jobId=' + this.JobId + '&name=' + this.name;
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.model = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        save: function (callback) {
            this.$refs['editForm'].validate(valid => {
                if (valid) {
                    var apiUrl = 'api/quartz/save';
                    wy.post(apiUrl, this.model).then((res) => {
                        callback();
                        this.$emit('searchData');
                        wy.showSuccessMssg(res);
                        this.loading = false;
                    }).catch((error) => {
                        wy.showErrorMssg(error);
                        this.loading = false;
                    });
                } else {
                    wy.showErrorMssg('请输入必填项');
                }
            });
        },
        create: function () {
            this.loading = false;
            this.model = {
                JobInfoId: '',
                JobName: '',
                JobGroup: '',
                JobDesc: '',
                Cron: '',
                CronDesc: '',
                JobClass: '',
            };
        },
        close: function () {
            this.loading = false;
            this.$emit('update:visible', false);
        }
    }
}
</script>
