<template>
<div id="userroleedit" class="userroleedit">
    <el-dialog title="角色信息" :visible.sync="visible" :before-close="close">
        <div style="margin-bottom:5px;">
            <el-transfer :titles="['角色列表', '用户角色']" filterable :filter-method="filterMethod" filter-placeholder="请输入角色名称" v-model="userroles" :data="rolesData" :loading="loading">
            </el-transfer>
        </div>
        <span slot="footer">
            <el-button @click="handleChange" :loading="loading">保存</el-button>
        </span>
    </el-dialog>
</div>
</template>

<script>
export default {
    data() {
        return {
            userroles: [],
            filterMethod(query, item) {
                return item.label.indexOf(query) > -1;
            },
            loading: false,
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
            require: true
        },
        UserInfoId: {
            type: String,
            require: true
        },
        rolesData: {
            type: Array,
            required: true
        }
    },
    computed: {

    },
    mounted: function () {
        if (this.UserInfoId) {
            this.load();
        } else {
            wy.showErrorMssg('用户Id丢失！');
        }
    },
    methods: {
        load: function () {
            var apiUrl = 'api/userinfo/rolelist?id=' + this.UserInfoId;
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.userroles = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        handleChange: function (callback) {
            var apiUrl = 'api/userinfo/saveuir';
            var data = {
                userInfoId: this.UserInfoId,
                roleInfoIds: this.userroles
            }
            this.loading = true;
            wy.post(apiUrl, data).then((res) => {
                wy.showSuccessMssg(res);
                this.$emit('searchData');
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        close: function () {
            this.loading = false;
            this.$emit('update:visible', false);
        }
    }
}
</script>