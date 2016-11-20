<template>

</template>

<script>
import { mapGetters, mapActions } from 'vuex'

export default {
  data () {
    return {
      moods: [
      { label: 'Lounge', image: 'http://image.noelshack.com/fichiers/2016/23/1465756669-party.png' },
      { label: 'Energy', image: 'http://image.noelshack.com/fichiers/2016/24/1465931485-moodchill.png' },
      { label: 'Dance', image: 'http://image.noelshack.com/fichiers/2016/24/1465931498-moodsport.png' },
      { label: 'Mad', image: 'http://image.noelshack.com/fichiers/2016/24/1465931510-moodwork.png' }]
    }
  },
  ready: function () {

  }
  // methods: {
  //   ...mapActions(['showModal'])
  // }
}
</script>
