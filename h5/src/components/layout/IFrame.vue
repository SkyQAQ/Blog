<template>
    <iframe v-if="visible" class="wy-iframe-menu" id="wy_iframe_menu" :src="src" scrolling="auto" frameborder="0">
    </iframe>
</template>

<script>
export default {
    data() {
        return {
            visible: true,
            src: '',
        };
    },
    mounted() {
        this.onload();
    },
    watch: {
        '$route': function() {
            this.onload();
        }
    },
    methods: {
        onload: function () {
            this.src = this.$route.query.src;
        }
    }
};
</script>
<style scoped>
    .wy-iframe-menu {
        width: 100%;
        height: 85%;
    }
</style>

