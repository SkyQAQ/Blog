<template>
<div id="home">
    This is my first page 你好: {{mssg}}
</div>
</template>

<script>
export default {
    data() {
        return {
            mssg: '',
        }
    },
    mounted: function () {
        //this.onload();
    },
    computed: {},
    methods: {
        onload: function () {
            this.mssg = this.$store.state.user.UserAccount;
            wy.get('api/home/createcoustommenu',{name:'222',url:'3333'}).then((res) => {
                this.model = res;
                this.loading = false;
            }).catch((error) => {
                wy.showErrorMssg(error);
                this.loading = false;
            });
        },
    }
}
</script>
