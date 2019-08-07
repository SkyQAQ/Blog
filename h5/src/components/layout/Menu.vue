<template>
<div id="menu">
    <el-menu :default-active="activeIndex" mode="horizontal" :router="true" background-color="#3B5B7B" text-color="#C0C4CC" active-text-color="#fff">
        <el-menu-item index="/home">首页</el-menu-item>
        <!-- 一级菜单 -->
        <template v-for="menu1 in menurouter">
            <el-menu-item :key="menu1.id" :index="menu1.label2" v-if="!menu1.children">{{menu1.label}}</el-menu-item>
            <el-submenu :key="menu1.id" :index="menu1.label2" v-else>
                <template slot="title">{{menu1.label}}</template>
                <!-- 二级菜单 -->
                <template v-for="menu2 in menu1.children">
                    <el-menu-item :key="menu2.id" :index="menu2.label2" v-if="!menu2.children">{{menu2.label}}</el-menu-item>
                    <el-submenu :key="menu2.id" :index="menu2.label2" v-else>
                        <template slot="title">{{menu2.label}}</template>
                        <!-- 三级菜单 -->
                        <template v-for="menu3 in menu2.children">
                            <el-menu-item :key="menu3.id" :index="menu3.label2" v-if="!menu3.children">{{menu3.label}}</el-menu-item>
                            <el-submenu :key="menu3.id" :index="menu3.label2" v-else>
                                <template slot="title">{{menu3.label}}</template>
                                <el-menu-item :key="menu3.id" :index="menu3.label2">{{menu3.label}}</el-menu-item>
                            </el-submenu>
                        </template>
                    </el-submenu>
                </template>
            </el-submenu>
        </template>
    </el-menu>
</div>
</template>

<script>
export default {
    name: 'Menu',
    data() {
        return {
            menurouter: [],
            activeIndex: '',
        }
    },
    mounted() {
        this.loadMenuRouter();
        this.loadActiveIndex();
    },
    watch: {
        '$route': 'loadActiveIndex',
    },
    components: {},
    methods: {
        loadMenuRouter: function () {
            var apiUrl = 'api/menuinfo/router';
            this.loading = true;
            wy.get(apiUrl).then((res) => {
                this.menurouter = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
        loadActiveIndex: function () {
            if (this.$route.path) {
                this.activeIndex = this.$route.path;
            } else {
                this.activeIndex = '/home';
            }
        }
    }
}
</script>

<style>
.el-menu--horizontal>.el-submenu .el-submenu__title {
    height: 50px;
}

.el-menu--horizontal>.el-menu-item {
    height: 50px;
}

.el-menu {
    margin-top: 0;
    margin-left: 0;
}
</style>
