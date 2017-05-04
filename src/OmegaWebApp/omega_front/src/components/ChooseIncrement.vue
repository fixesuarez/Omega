<template>
    <div>
        <input type="number" v-model="choice">
        <button @click="add" type="button" class="btn btn-primary">Ok</button>

        <input type="text" v-model="text">
        <button @click="addText" type="button" class="btn btn-primary">Ok</button>

        <a href="#" class="playlistsTab" v-on:click="addText('playlistsTab')">playlists</a><br>
    </div>
</template>

<script>

export default {
    data () { 
        return {
            choice: '',
            text: ''
        }
    },
    computed: {
        count() {
            return this.$store.getters.count
        }
    },
    methods: {
        add() {
            this.$store.dispatch('add', { choice: this.choice });
        },
        addText(item) {
          this.text = item;
          this.$store.dispatch('addText', { text: this.text });
        }
    }
}
</script>