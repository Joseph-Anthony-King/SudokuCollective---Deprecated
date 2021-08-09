<template>
  <div>
    <v-card elevation="6" class="mx-auto">
      <v-container>
        <span @click="closeReviewUserWidget" class="material-icons close-hover">
          clear
        </span>
      </v-container>
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">
            Review User: {{ user.userName }}
          </v-card-title>
          <hr class="title-spacer" />
          <UserProfileWidget :user="user" />
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
  </div>
</template>

<style scoped></style>

<script>
/* eslint-disable no-unused-vars */
import { mapGetters } from "vuex";
import User from "@/models/user";
import UserProfileWidget from "@/components/widgets/UserProfileWidget";

export default {
  name: "ReviewUserWidget",
  components: {
    UserProfileWidget,
  },
  data: () => ({
    user: new User(),
  }),
  methods: {
    closeReviewUserWidget() {
      this.$emit("close-review-user-widget-event", null, null);
    },
  },
  computed: {
    ...mapGetters("userModule", ["getSelectedUser"]),
  },
  watch: {
    "$store.state.userModule.selectedUser": {
      handler: function (val, oldVal) {
        this.$data.user = this.getSelectedUser;
      },
    },
  },
  created() {
    this.$data.user = this.getSelectedUser;
  },
};
</script>
