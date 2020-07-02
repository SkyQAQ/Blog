<template>
<div id="home">
    <wy-header :titles="titles">
    </wy-header>
    <wy-content>
        
    </wy-content>
</div>
</template>

<script>
export default {
    data() {
        return {
            titles: ['首页', '世界名著'],
        }
    },
    mounted: function () {
        this.onload();
    },
    computed: {},
    methods: {
        onload: function () {
            this.mssg = this.$store.state.user.UserAccount;
        },
    }
}
</script>
