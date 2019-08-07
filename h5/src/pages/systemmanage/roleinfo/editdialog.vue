<template>
<div id="roleinfoedit">
    <el-dialog title="角色信息" :visible.sync="visible" :before-close="close" width="30%">
        <el-form ref="editForm" :rules="rules" :model="model" label-width="100px">
            <el-form-item label="角色名称" prop="RoleName">
                <el-input type="text" v-model="model.RoleName" auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="角色编码" prop="RoleCode">
                <el-input type="text" v-model="model.RoleCode" auto-complete="off">
                </el-input>
            </el-form-item>
        </el-form>
        <span slot="footer">
            <el-button @click="save(create)" :loading="loading" style="width:120px" v-show="!this.RoleInfoId">保存并新建</el-button>
            <el-button @click="close" style="width:120px" :loading="loading" v-show="this.RoleInfoId">关闭</el-button>
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
                RoleInfoId: '',
                RoleName: '',
                RoleCode: ''
            },
            loading: false,
            rules: {
                RoleName: [{
                    required: true,
                    message: '请输入角色名称',
                    trigger: 'blur'
                }],
                RoleCode: [{
                        required: true,
                        message: '请输入角色编码',
                        trigger: 'blur'
                    },
                    // { min: 8, max: 16, message: '长度在 8 到 16 个字符', trigger: 'blur' }
                ]
            },
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
            require: true
        },
        RoleInfoId: {
            type: String,
            default: '',
            require: false
        },
    },
    computed: {

    },
    mounted: function () {
        if (this.RoleInfoId) {
            this.load();
        }
    },
    methods: {
        load: function () {
            var apiUrl = 'api/roleinfo/edit?id=' + this.RoleInfoId;
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
                    var apiUrl = 'api/roleinfo/save';
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
                RoleInfoId: '',
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
