<template>
<div id="dreaminfoedit">
    <el-dialog title="梦想信息" :visible.sync="visible" :before-close="close" :close-on-click-modal="false" width="30%">
        <el-form ref="editForm" :rules="rules" :model="model" label-width="100px">
            <el-form-item label="彩票类型" prop="Type">
                <el-select placeholder="请选择" v-model="model.Type">
                    <el-option v-for="item in DreamType" :key="item.Value" :label="item.Text" :value="item.Value">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="梦想编码" prop="DreamCode">
                <el-input v-model="model.DreamCode" clearable></el-input>
            </el-form-item>
            <el-form-item label="起始期数" prop="StartStage">
                <el-input v-model="model.StartStage" type="number" clearable></el-input>
            </el-form-item>
            <el-form-item label="截止期数" prop="EndStage">
                <el-input v-model="model.EndStage" type="number" clearable></el-input>
            </el-form-item>
        </el-form>
        <span slot="footer">
            <el-button @click="save(create)" :loading="loading" style="width:120px" v-show="!this.DreamInfoId">保存并新建</el-button>
            <el-button @click="close" style="width:120px" :loading="loading" v-show="this.DreamInfoId">关闭</el-button>
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
                DreamInfoId: '',
                DreamCode: '',
                Type: 1,
                StartStage: '',
                EndStage: '',
            },
            loading: false,
            rules: {
                DreamCode: [{
                    required: true,
                    message: '请输入梦想名称',
                    trigger: 'blur'
                }],
                StartStage: [{
                    required: true,
                    message: '请输入起始期数',
                    trigger: 'blur'
                }],
                EndStage: [{
                    required: true,
                    message: '请输入截止期数',
                    trigger: 'blur'
                }],
            },
            DreamType: [{
                    Text: '大乐透',
                    Value: 1
                },
                {
                    Text: '排列三',
                    Value: 2
                },
                {
                    Text: '排列五',
                    Value: 3
                },
            ],
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
            require: true
        },
        DreamInfoId: {
            type: String,
            default: '',
            require: false
        },
    },
    computed: {

    },
    mounted: function () {
        if (this.DreamInfoId) {
            this.load();
        }
    },
    methods: {
        load: function () {
            var apiUrl = 'api/dreaminfo/edit?id=' + this.DreamInfoId;
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
                    var apiUrl = 'api/dreaminfo/save';
                    this.loading = true;
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
                DreamInfoId: '',
                RoleName: '',
                RoleCode: ''
            };
        },
        close: function () {
            this.loading = false;
            this.$emit('update:visible', false);
        }
    }
}
</script>
