<template>
  <div>
    <button type="button" @click="loadFacebookGroup()">Groups</button>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import FacebookApiService from '../services/FacebookApiService'

export default {
  data() {
    return {
        groups:[]
    }
  },
  methods: {
    ...mapActions(['requestAsync']),
    loadFacebookGroup: async function() {
        var groups = await this.requestAsync(() => FacebookApiService.getFacebookGroup());
        this.groups = groups;
    }
  }
}
</script>