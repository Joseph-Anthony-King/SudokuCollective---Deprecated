<template>
  <v-card>
    <v-card-title>
      <span class="headline">Login</span>
    </v-card-title>
    <v-form v-model="formIsValid">
      <v-card-text>
        <v-container>
          <v-row>
            <v-col cols="12">
              <v-text-field
                v-model="username"
                label="User Name"
                prepend-icon="mdi-account circle"
                :rules="userNameRequired"
                required
              ></v-text-field>
            </v-col>
            <v-col cols="12">
              <v-text-field
                v-model="password"
                label="Password"
                :type="showPassword ? 'text' : 'password'"
                prepend-icon="mdi-lock"
                :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append="showPassword = !showPassword"
                :rules="passwordRequired"
                required
              ></v-text-field>
            </v-col>
          </v-row>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="close"> Close </v-btn>
        <v-btn
          color="blue darken-1"
          text
          @click="authenticate"
          :disabled="!formIsValid"
        >
          Login
        </v-btn>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script>
import { authenticationService } from "../services/authenticationService/authentication.service";
import User from "../models/user";

export default {
  name: "LoginForm",
  props: ["userForAuthentication", "loginFormStatus"],
  data: () => ({
    username: "",
    password: "",
    user: {},
    formIsValid: true,
    showPassword: false,
    userNameRequired: [(v) => !!v || "User Name is required"],
    passwordRequired: [(v) => !!v || "Password is required"],
  }),
  methods: {
    async authenticate() {
      if (this.getloginFormStatus) {
        try {
          const response = await authenticationService.authenticateUser(
            this.$data.username,
            this.$data.password
          );

          if (response.status === 200) {
            this.$data.user.shallowClone(response.data.user);

            await this.resetData();

            this.$emit(
              "user-logging-in-event",
              this.$data.user,
              response.data.token
            );

            this.resetLoginFormStatus;

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

  computed: {
    getloginFormStatus() {
      return this.loginFormStatus;
    },
    resetLoginFormStatus() {
      return !this.loginFormStatus;
    }
  },

  mounted() {
    let self = this;

    window.addEventListener("keyup", function (event) {
      if (event.keyCode === 13) {
        self.authenticate();
      }
    });

    this.$data.user = new User();
    this.$data.user.shallowClone(this.$props.userForAuthentication);
  },
};
</script>