<template>
  <div>
    <button id="left">Left</button>
    <button id="right">Right</button>
    <button id='upClick'>Go Up</button> <button id='downClick'>Go Down</button><button class='clearValue'>Clear Value</button> 


<div class='cover'><div class='rightSection'></div></div>
  </div>
</template>

<script>
import $ from 'jquery'

var scrolled=0;

$("document").ready(function(){
           console.log('event scroll = '+ scrolled);

    	
    $("#downClick").on("click" ,function(){
                scrolled=scrolled+300;
        
				$(".cover").animate({
				        scrollLeft:  scrolled
				   });
           console.log(scrolled);
			});

    
    $("#upClick").on("click" ,function(){
				scrolled=scrolled-300;
				
				$(".cover").animate({
				        scrollLeft:  scrolled
				   });
           console.log(scrolled);

			});


$(".clearValue").on("click" ,function(){
				scrolled=0;
		});


});



export default {
  data () {
    return {
      playlists: [{"id":0,"name":"Apero Party","image":"http://d817ypd61vbww.cloudfront.net/sites/default/files/styles/media_responsive_widest/public/tile/image/A-116-01comp.jpg?itok=LjWmwzjU"},{"id":1,"name":"Mexico","image":"https://caliloved.files.wordpress.com/2013/07/deer-album-cover-new.jpg"},{"id":2,"name":"Chanson francaise","image":"http://www.designformusic.com/wp-content/uploads/2016/01/perfectly-chilled-album-cover-artwork-design-500x500.jpg"},{"id":3,"name":"Cam box","image":"https://www.smashingmagazine.com/images/music-cd-covers/64.jpg"},{"id":4,"name":"Jaccuzi money billey","image":"https://img.buzzfeed.com/buzzfeed-static/static/2016-01/27/11/enhanced/webdr14/enhanced-6784-1453912540-22.jpg"},{"id":5,"name":"Beer-Pong","image":"http://androidjones.com/wp-content/uploads/2012/05/HOPE-1024x1024.jpg"},{"id":6,"name":"Runing Time","image":"http://illusion.scene360.com/wp-content/uploads/2014/10/computergraphics-album-covers-2014-05.jpg"},{"id":7,"name":"Soiree OKLM","image":"http://www.fuse.tv/image/5682ea90ac0e76bd68000055/768/512/brown-eyed-girls-basic-album-cover-full-size.jpg"},{"id":8,"name":"Apero Party","image":"http://takuya.fr/wp-content/uploads/2016/06/takuya-fr-thedoubt.jpg"}],
      test: 'Test Reussi'
    }
  },
  methods: {
  },
  computed: {
  },
  components: {
  }
}
</script>

<style>
.cover{
    width:500px;
    height:100px;
    background-color:#000000;
    overflow:auto;
}


.rightSection{
    width:3300px;
    height:100px;
}

</style>