<template>
<div id="rolemenuedit">
    <el-dialog title="配置菜单" :visible.sync="visible" :before-close="close" width="30%">
        <div style="height:60%;overflow-y:auto; overflow-x:auto;margin-bottom:20px">
            <el-input v-model="filterText"></el-input>
            <el-tree ref="menutree" :data="menutreedata" :filter-node-method="filterNode" show-checkbox node-key="id" :default-checked-keys="checkedid" :check-strictly="true">
            </el-tree>
        </div>
        <span slot="footer">
                <el-button :loading="loading" @click="close">关闭</el-button>
                <el-button type="primary" :loading="loading" @click="save">保存</el-button>
            </span>
    </el-dialog>
</div>
</template>

<script>
export default {
    data() {
        return {
            loading: false,
            filterText: '',
            checkedid: [],
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
            require: true
        },
        menutreedata: {
            type: Array,
            required: true
        }
    },
    watch: {
        filterText(val) {
            this.$refs.menutree.filter(val);
        }
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
            var apiUrl = 'api/roleinfo/menulist?id=' + this.RoleInfoId;
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.checkedid = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        save: function () {
            var apiUrl = 'api/roleinfo/savemir';
            var data = {
                roleInfoId: this.RoleInfoId,
                menuInfoIds: this.$refs.menutree.getCheckedKeys()
            }
            this.loading = true;
            wy.post(apiUrl, data).then((res) => {
                wy.showSuccessMssg(res);
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        close: function () {
            this.loading = false;
            this.$emit('update:visible', false);
        },
        filterNode(value, data) {
            if (!value) return true;
            return data.label.indexOf(value) !== -1;
        },
    }
}
</script>
