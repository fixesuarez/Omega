<template>
  <div>
    <Login></Login>
    <moods></moods>
  </div>
</template>

<script>
  import Login from './components/Login.vue'
  import moods from './components/moods.vue'
  export default {
  data () {
    return {
    }
  },
  methods: {
  },
  name: 'app',
  components: {
    Login,
    moods
  }
}
</script>
