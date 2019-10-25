<template>
<div id="userinfoedit">
    <el-dialog title="用户信息" :visible.sync="visible" :before-close="close" width="30%" :modal="false" :close-on-click-modal="false">
        <el-form ref="editForm" :rules="rules" :model="model" label-width="100px">
            <el-form-item label="账号" prop="Account">
                <el-input type="text" v-model="model.Account" disabled auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="邮箱" prop="Email">
                <el-input type="text" v-model="model.Email" disabled auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="名称" prop="Name">
                <el-input type="text" v-model="model.Name" auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="密码" prop="Password">
                <el-input type="password" v-model="model.Password" auto-complete="off">
                </el-input>
            </el-form-item>
            <el-form-item label="确认密码" prop="ConfirmPassword">
                <el-input type="password" v-model="model.ConfirmPassword" auto-complete="off">
                </el-input>
            </el-form-item>
        </el-form>
        <span slot="footer">
            <el-button @click="close" style="width:120px" :loading="loading">关闭</el-button>
            <el-button @click="save(saveSuccess)" type="primary" :loading="loading" style="width:120px">保存</el-button>
        </span>
    </el-dialog>
</div>
</template>

<script>
export default {
    data() {
        var validatePass = (rule, value, callback) => {
            if (this.model.Password !== '') {
                var valid = wy.ValidPassword(this.model.Password);
                if (valid !== '') {
                    callback(new Error(valid));
                }
            }
        };
        var validatePass2 = (rule, value, callback) => {
            if (this.model.Password !== '' && value === '') {
                callback(new Error('请再次输入密码'));
            } else if (value !== this.model.Password) {
                callback(new Error('两次输入密码不一致!'));
            } else {
                callback();
            }
        };
        return {
            model: {
                Account: '',
                Email: '',
                Name: '',
                Password: '',
                ConfirmPassword: '',
            },
            loading: false,
            rules: {
                Password: [{
                    validator: validatePass,
                    trigger: 'blur'
                }],
                ConfirmPassword: [{
                    validator: validatePass2,
                    trigger: 'blur'
                }],
            },
            passwordKey: '',// 密码加密Key
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
            require: true
        },
    },
    computed: {

    },
    mounted: function () {
        this.load();
        this.getPublicKey();
    },
    methods: {
        load: function () {
            var user = this.$store.state.user;
            this.model.Account = user.UserAccount;
            this.model.Email = user.UserEmail;
            this.model.Name = user.UserName;
        },
        save: function (callback) {
            var apiUrl = 'api/userinfo/save?name=' + this.model.Name + '&pwd=' + wy.rsa(this.model.Password, this.passwordKey);
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                callback();
                wy.showSuccessMssg(res);
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        saveSuccess: function () {
            this.$store.commit('update_user_name', this.model.Name);
        },
        getPublicKey: function() {
            wy.get('api/auth/publickey').then((res) => {
                this.passwordKey = res;
            }).catch((error) => {
                wy.showErrorMssg(error);
            });
        },
        close: function () {
            this.loading = false;
            this.$emit('update:visible', false);
        }
    }
}
</script>
