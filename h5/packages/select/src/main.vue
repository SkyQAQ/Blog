<template>
    <el-select v-model="selectedValue" :disabled="disabled" :multiple="multiple " :clearable="clearable" @change="handleChange">
        <el-option v-for="item in options" :key="item.Value" :label="item.Text" :value="item.Value" :disabled="item.disabled">
            {{item.Text}}
        </el-option>
    </el-select>
</template>

<script type="text/javascript">
export default {
    name: "WySelect",
    props: {
        value: {
            type: String
        },
        except: {
            type: Array,
            default: function() {
                return [];
            },
            required: false
        },
        disabled: {
            type: Boolean,
            default: false,
            required: false
        },
        clearable: {
            type: Boolean,
            default: true,
            required: false
        },
        multiple: {
            type: Boolean,
            default: false,
            required: false
        },
        apiUrl: {
            type: String,
            default: '',
            required: false
        }
    },
    data() {
        return {
            selectedValue: [],
            options: [],
        }
    },
    watch: {
        value(newVal, oldVal) {
            this.selectedValue = newVal;
        }
    },
    mounted: function () {
        this.bindPicklist();
    },
    methods: {
        bindPicklist: function () {
            var self = this;
            wy.get(self.apiUrl)
                .then(function (res) {
                    self.options = res;
                    if(self.value){
                        self.selectedValue = self.value;
                    }
                    if(self.except && self.except.length > 0){
                        self.except.forEach(element => {
                            for(var i in self.options){
                                if(self.options[i].Value == element){
                                    self.options.splice(i, 1);
                                }
                            }  
                        });
                    }
                });
        },
        handleChange(val) {
            this.$emit("input", val);
            this.$emit("change",val);
        }
    }
}
</script>