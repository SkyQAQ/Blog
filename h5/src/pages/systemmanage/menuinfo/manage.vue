<template>
<div id="menuinfomanage">
    <wy-header :titles="titles">
    </wy-header>
    <wy-content>
        <el-container>
            <el-aside>
                <div style="overflow-y:auto;height:-webkit-fill-available;">
                    <el-input v-model="filterText"></el-input>
                    <el-tree ref="menutree" :data="menutreedata" :expand-on-click-node="false" :filter-node-method="filterNode" node-key="id" :check-strictly="true" @node-click="load" default-expand-all :highlight-current="true">
                    </el-tree>
                </div>
            </el-aside>
            <el-container>
                <el-header>
                    <el-button type="text" icon="el-icon-plus" @click="create">新建</el-button>
                    <el-button type="text" icon="el-icon-check" @click="save">保存</el-button>
                    <el-button type="text" icon="el-icon-delete" @click="deletes">删除</el-button>
                    <div class="wy-divider-x"></div>
                </el-header>
                <el-main>
                    <el-form ref="editForm" :rules="rules" :model="model" label-position="left" label-width="100px">
                        <el-row :gutter="40">
                            <el-col :span="8">
                                <el-form-item label="菜单名称" prop="MenuName">
                                    <el-input type="text" v-model="model.MenuName"></el-input>
                                </el-form-item>
                            </el-col>
                            <el-col :span="8">
                                <el-form-item label="菜单编码" prop="MenuCode">
                                    <el-input type="text" v-model="model.MenuCode"></el-input>
                                </el-form-item>
                            </el-col>
                        </el-row>
                        <el-row :gutter="40">
                            <el-col :span="8">
                                <el-form-item label="菜单路径" prop="MenuPath">
                                    <el-input type="text" v-model="model.MenuPath"></el-input>
                                </el-form-item>
                            </el-col>
                            <el-col :span="8">
                                <el-form-item label="菜单排序" prop="MenuSeq">
                                    <el-input type="text" v-model="model.MenuSeq"></el-input>
                                </el-form-item>
                            </el-col>
                        </el-row>
                        <el-row :gutter="40">
                            <el-col :span="16">
                                <el-form-item label="上级菜单" prop="PMenuId">
                                    <wy-input-tree type="text" v-model="model.PMenuId" :disabled-node="model.MenuInfoId" :treedata="menutreedata"></wy-input-tree>
                                </el-form-item>
                            </el-col>
                            <el-col :span="8" v-show="false">
                                <el-form-item label="菜单Id" prop="MenuInfoId">
                                    <el-input type="text" v-model="model.MenuInfoId"></el-input>
                                </el-form-item>
                            </el-col>
                        </el-row>
                    </el-form>
                </el-main>
            </el-container>
        </el-container>
    </wy-content>
</div>
</template>

<script>
export default {
    data() {
        return {
            titles: ['系统管理', '菜单管理', '菜单编辑'],
            menutreedata: [],
            filterText: '',
            model: {
                MenuInfoId: '',
                PMenuId: {
                    Id: '',
                    Name: ''
                },
                MenuName: '',
                MenuCode: '',
                MenuPath: '',
                MenuSeq: '',
            },
            loading: false,
            rules: {
                MenuName: [{
                    required: true,
                    message: '请输入菜单名称',
                    trigger: 'blur'
                }],
                MenuCode: [{
                    required: true,
                    message: '请输入菜单编码',
                    trigger: 'blur'
                }, ],
                MenuPath: [{
                    required: true,
                    message: '请输入菜单路径',
                    trigger: 'blur'
                }],
                MenuSeq: [{
                    required: true,
                    message: '请输入菜单排序',
                    trigger: 'blur'
                }, ],
            },
        }
    },
    props: {

    },
    mounted: function () {
        this.loadMenuTreeData();
    },
    computed: {

    },
    watch: {
        filterText(val) {
            this.$refs.menutree.filter(val);
        }
    },
    methods: {
        load: function (data) {
            var apiUrl = 'api/menuinfo/edit?id=' + data.id;
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.model = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        create: function () {
            this.$refs['editForm'].resetFields();
        },
        save: function () {
            this.$refs['editForm'].validate(valid => {
                if (valid) {
                    var apiUrl = 'api/menuinfo/save';
                    this.loading = true;
                    wy.post(apiUrl, this.model).then((res) => {
                        this.loadMenuTreeData();
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
        deletes: function () {
            if (!this.model.MenuInfoId) {
                wy.showErrorMssg('当前菜单未创建无需删除！');
                return;
            }
            var apiUrl = 'api/menuinfo/delete?id=' + this.model.MenuInfoId;
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.loadMenuTreeData();
                this.create();
                wy.showSuccessMssg(res);
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        loadMenuTreeData: function () {
            var apiUrl = 'api/menuinfo/treedata';
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.menutreedata = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        filterNode(value, data) {
            if (!value) return true;
            return data.label.indexOf(value) !== -1;
        },
    }
}
</script>

<style>

</style>
