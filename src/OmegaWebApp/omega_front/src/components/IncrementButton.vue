<template>
    <div>
        <button @click="increment" type="button" class="btn btn-primary">+1</button>
        <button @click="decrement" type="button" class="btn btn-primary">-1</button>
        <button @click="update" type="button" class="btn btn-primary">update</button>

    </div>
</template>

<script>

export default {
    computed: {
        count() {
            return this.$store.getters.count
        }
    },
    methods: {
        increment() {
            this.$store.dispatch('increment')
        },
        decrement() {
            this.$store.dispatch('decrement')
        },
         update() {
            this.$store.dispatch('update')
        }
    }
}
</script>