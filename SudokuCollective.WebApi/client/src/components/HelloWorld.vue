<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <h2>{{ apiMsg }}</h2>
  </div>
</template>

<script>
import * as axios from 'axios'

export default {
  name: "HelloWorld",
  props: {
    msg: String
  },
  data() {
    return {
      apiMsg: ''
    }
  },
  async mounted() {
    
    // When running the api in kestrel...
    const response = await axios.get('https://localhost:5001/api/v1/helloworld', { crossdomain: true });

    // When running the api in IIS Express...
    //const response = await axios.get('https://localhost:44371/api/v1/helloworld', { crossdomain: true });

    console.log('api response: ', response);
    this.$data.apiMsg = response.data;
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
