<template>
  <v-card>
    <v-card-title>
      <span class="headline">Login</span>
    </v-card-title>
    <v-card-text>
      <v-container>
        <v-row>
          <v-col cols="12">
            <v-text-field
              v-model="username"
              label="User Name"
              required
            ></v-text-field>
          </v-col>
          <v-col cols="12">
            <v-text-field
              v-model="password"
              label="Password"
              type="password"
              required
            ></v-text-field>
          </v-col>
        </v-row>
      </v-container>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="blue darken-1" text @click="close"> Close </v-btn>
      <v-btn color="blue darken-1" text @click="authenticate"> Login </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import { authenticationService } from "../services/authenticationService/authentication.service";
import User from "../models/user";

export default {
  name: "LoginForm",
  props: ["userForAuthentication"],
  data: () => ({
    username: "",
    password: "",
    user: {},
  }),
  methods: {
    async authenticate() {
      try {
        const response = await authenticationService.authenticateUser(
          this.$data.username,
          this.$data.password
        );

        if (response.status === 200) {
          this.$data.user.shallowClone(response.data.user);

          await this.resetData();

          this.$emit("user-logging-in-event", this.$data.user, response.data.token);

          this.$toasted.success(`${this.$data.user.fullName} is logged in.`, {
            duration: 3000,
          });
        } else if (response.status === 400) {
          this.$toasted.error("Username or Password is incorrect.", {
            duration: 3000,
          });
        } else {
          this.$toasted.error(
            "An error occurred while trying to authenticate the user",
            {
              duration: 3000,
            }
          );
        }
      } catch (error) {
        this.$toasted.error(error, {
          duration: 3000,
        });
      }
    },

    close() {
      this.$emit("user-logging-in-event", null, null);
    },

    async resetData() {
      this.$data.username = "";
      this.$data.password = "";
    },
  },
  mounted() {
    this.$data.user = new User();
    this.$data.user.shallowClone(this.$props.userForAuthentication);
  },
};
</script>