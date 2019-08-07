<template>
<div class="el-menu--horizontal">
    <template v-for="menu in menurouter">
        <el-menu-item :key="menu.id" :index="menu.label2" v-if="!menu.children">{{menu.label}}</el-menu-item>
        <el-submenu :key="menu.id" :index="menu.label2" v-else>
            <template slot="title">{{menu.label}}</template>
            <wy-menu-item v-if="menu.children" :menurouter="menu.children"></wy-menu-item>
        </el-submenu>
    </template>
</div>
</template>

<script>
import WyMenuItem from './menuitem.vue'

export default {
    name: 'WyMenuItem',
    data() {
        return {}
    },
    props: {
        menurouter: {
            type: Array,
            require: true
        }
    },
    components: {
        WyMenuItem,
    },
    mounted: function () {},
    computed: {

    },
    methods: {

    }
}
</script>

<style>
</style>
