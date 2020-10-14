<template>
  <v-form>
    <v-container>
        <div v-if="!user.isLoggedIn">
            <h1>Login</h1>
            <v-row>
                <v-col cols="12">
                    <v-text-field prepend-icon="person"
                                  name="Username"
                                  label="Username"
                                  required
                                  outlined
                                  v-model="username"></v-text-field>
                    <v-text-field prepend-icon="lock"
                                  name="Password"
                                  label="Password"
                                  required
                                  outlined
                                  v-model="password"></v-text-field>
                    <v-card-actions>
                        <v-flex justify-center>
                            <v-btn @click="submit()" color="primary">Login</v-btn>
                        </v-flex>
                    </v-card-actions>
                </v-col>
            </v-row>

        </div>
        <div v-if="user.isLoggedIn">
            <h1 class="display-2 font-weight-bold mb-3">
                {{ apiMsg }}
            </h1>
            <h2>{{ user.userName }} is logged in!</h2>
            <v-row>
                <v-col cols="12">
                    <v-text-field prepend-icon="person"
                                  name="user_userName"
                                  label="Username"
                                  disabled
                                  outlined
                                  v-model="user.userName"></v-text-field>
                    <v-text-field prepend-icon="person"
                                  name="user_firstName"
                                  label="First Name"
                                  disabled
                                  outlined
                                  v-model="user.firstName"></v-text-field>
                    <v-text-field prepend-icon="person"
                                  name="user_lastName"
                                  label="Last Name"
                                  disabled
                                  outlined
                                  v-model="user.lastName"></v-text-field>
                    <v-text-field prepend-icon="person"
                                  name="user_fullName"
                                  label="Fullname"
                                  disabled
                                  outlined
                                  v-model="user.fullName"></v-text-field>
                    <v-text-field prepend-icon="person"
                                  name="user_nickName"
                                  label="Nickname"
                                  disabled
                                  outlined
                                  v-model="user.nickName"></v-text-field>
                    <v-text-field prepend-icon="email"
                                  name="user_email"
                                  label="email"
                                  disabled
                                  outlined
                                  v-model="user.email"></v-text-field>
                    <v-checkbox name="user_isActive"
                                label="Is Active"
                                v-model="user.isActive"
                                disabled></v-checkbox>
                    <v-checkbox name="user_isAdmin"
                                label="Is Admin"
                                v-model="user.isAdmin"
                                disabled></v-checkbox>
                    <v-checkbox name="user_isSuperUser"
                                label="Is Super User"
                                v-model="user.isSuperUser"
                                disabled></v-checkbox>
                    <v-card-actions>
                        <v-flex justify-center>
                            <v-btn @click="logout()" color="primary">Logout</v-btn>
                        </v-flex>
                    </v-card-actions>
                </v-col>
            </v-row>
        </div>
    </v-container>
  </v-form>
</template>

<script>
    import { mapGetters } from "vuex";
    import { userService } from "../services/user.service";
    import { authenticationService } from "../services/authentication.service";
    import User from "../models/user"

    export default {

        name: "LoginForm",

        data: () => ({
            apiMsg: "",
            username: "",
            password: "",
            user: {}
        }),

        methods: {

            async submit() {

                try {

                    const response = await authenticationService.authenticateUser(
                        this.$data.username, 
                        this.$data.password);

                    if (response.status === 200) {

                        this.$data.user.shallowClone(response.data.user);

                        this.$data.user = userService.loginUser(
                            this.$data.user, 
                            response.data.token);

                        alert(`${this.$data.user.fullName} is logged in.`);

                    } else if (response.status === 400) {

                        alert("Username or Password is incorrect.");

                    } else {

                        alert("An error occurred while trying to authenticate the user");
                    }

                } catch (error) {

                    alert(error);
                }
            },

            logout() {

                const userFullName = this.$data.user.fullName;

                this.$data.user = userService.logoutUser(this.$data.user);

                this.resetData();

                alert(`${userFullName} has been logged out.`);
            },

            resetData() {
                this.$data.username = "";
                this.$data.password = "";
            }
        },

        computed: {
            ...mapGetters("appSettingsModule", ["getAPIMessage"]),
            ...mapGetters("userModule", ["getUser"])
        },

        created() {
            this.$data.apiMsg = this.$store.getters["appSettingsModule/getAPIMessage"];
        },

        mounted() {
            this.$data.user = new User();
            this.$data.user.shallowClone(this.getUser);
        },

        updated() {
            this.$data.apiMsg = this.$store.getters["appSettingsModule/getAPIMessage"];
        }
    };
</script>