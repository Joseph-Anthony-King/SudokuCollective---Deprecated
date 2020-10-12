<template>
  <v-form>
    <v-container>
      <h1>Login</h1>
      <v-row>
        <v-col cols="12">
          <v-text-field
            prepend-icon="person"
            name="Username"
            label="Username"
            required
            outlined
            v-model="username"
          ></v-text-field>
          <v-text-field
            prepend-icon="lock"
            name="Password"
            label="Password"
            required
            outlined
            v-model="password"
          ></v-text-field>
          <v-card-actions>
            <v-flex justify-center>
              <v-btn @click="submit()" color="primary">Login</v-btn>
            </v-flex>
          </v-card-actions>
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>

<script>
    import * as axios from "axios";

    export default {

        name: "LoginForm",

        data: () => ({
            username: "",
            password: ""
        }),

        methods: {

            async submit() {

                try {

                    const config = {
                        method: "post",
                        url: `${this.$store.getters.getBaseURL}/api/v1/authenticate`,
                        headers: {
                            "Content-Type": "application/json"
                        },
                        data: {
                            UserName: `${this.$data.username}`,
                            Password: `${this.$data.password}`
                        }
                    };

                    const response = await axios(config);

                    var user = response.data.user;

                    alert(`${user.fullName} is now logged in!`);

                } catch (error) {

                    alert(error);
                }
            }
        }
    };
</script>