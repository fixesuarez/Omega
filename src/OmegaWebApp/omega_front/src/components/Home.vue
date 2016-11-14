<template>
  <div id="app">
    <html>
      <body>
		
        <button type="button" @click="http://localhost:5000/ExternalLogin(Spotify)" class="btn btn-lg btn-primary">Spotify</button>

      </body>
    </html>
  </div>
</template>

<script>
export default {
  data () {
    return {
      active: 'playlistsTab',
      activePlayer: '',
      modalActive: false,
      playlistsTab: 'playlistsTab',
      evenementsTab: 'evenementsTab',
      groupesTab: 'groupesTab'
    }
  },
  methods: {
    makeActive: function(item) {
      this.active = item;
      this.$store.dispatch('makeActive', { active: this.active });
    },
    makeActivePlayer: function(item) {
      this.activePlayer = item;
    },
    makeModalActive: function() {
      this.modalActive = !this.modalActive
      this.$emit('input', this.modalActive)
    }
  },
  computed: {
  }
}
</script>

<style>
</style>