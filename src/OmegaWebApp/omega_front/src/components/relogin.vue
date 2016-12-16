<template>
    <div>
        <div class="text-center">
            <div class="page-header">
                <h1>Compléter votre login</h1>
            </div>

            <button type="button" @click="relogin('Facebook')" class="btn btn-lg btn-block btn-primary"><i class="fa fa-google" aria-hidden="true"></i> Se connecter via Facebook</button>
            <button type="button" @click="relogin('Spotify')" class="btn btn-lg btn-block btn-primary"><i class="fa fa-github" aria-hidden="true"></i> Se connecter via Spotify</button>            
            <button type="button" @click="relogin('Deezer')" class="btn btn-lg btn-block btn-primary"><i class="fa fa-github" aria-hidden="true"></i> Se connecter via Deezer</button>
        </div>
    </div>
</template>

<script>
    import AuthService from '../services/AuthService'
    import Vue from 'vue'
    import $ from 'jquery'

    export default {
        data() {
            return {
                endpoint: null
            }
        },

        created() {
            AuthService.registerAuthenticatedCallback(this.onAuthenticated);
        },

        beforeDestroy() {
            AuthService.removeAuthenticatedCallback(this.onAuthenticated);
        },

        methods: {
            async relogin(provider) {
                await AuthService.relogin(provider);
            },

            onAuthenticated() {
                 this.$router.replace('/');
            }
        }
    }
</script>

