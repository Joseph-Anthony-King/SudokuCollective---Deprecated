<template>
  <v-row>
    <v-col cols="12" lg="6" xl="6">
      <v-text-field
        v-model="userId"
        label="Id"
        prepend-icon="mdi-account-circle"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="userName"
        label="User Name"
        prepend-icon="mdi-account-circle"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="userFirstName"
        label="First Name"
        prepend-icon="mdi-account-circle"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="userLastName"
        label="Last Name"
        prepend-icon="mdi-account-circle"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="userNickName"
        label="Nickname"
        prepend-icon="mdi-account-circle"
        readonly
      ></v-text-field>
      <v-checkbox
        v-model="userIsAdmin"
        label="Admin Privileges"
        readonly
      ></v-checkbox>
      <v-checkbox
        v-if="userIsSuperUser"
        v-model="userIsSuperUser"
        label="Super User Privileges"
        readonly
      ></v-checkbox>
    </v-col>
    <v-col cols="12" lg="6" xl="6">
      <v-text-field
        v-model="userDateCreated"
        label="Date Created"
        hint="MM/DD/YYYY format"
        persistent-hint
        prepend-icon="mdi-calendar"
        readonly
      ></v-text-field>
      <v-text-field
        v-if="userDateUpdated !== '12/31/1, 4:07:02 PM'"
        v-model="userDateUpdated"
        label="Date Updated"
        hint="MM/DD/YYYY format"
        persistent-hint
        prepend-icon="mdi-calendar"
        readonly
      ></v-text-field>
      <v-text-field
        v-model="userEmail"
        label="Email"
        readonly
        prepend-icon="mdi-email"
      ></v-text-field>
      <v-checkbox
        v-model="userEmailConfirmed"
        label="Email Confirmed"
        readonly
      ></v-checkbox>
    </v-col>
  </v-row>  
</template>

<style scoped>
</style>

<script>
export default {
  name: "UserProfileWidget",
  props: {
    user: {}
  },
  computed: {
    userId() {
      return this.$props.user.id
    },
    userName() {
      return this.$props.user.userName
    },
    userFirstName() {
      return this.$props.user.firstName
    },
    userLastName() {
      return this.$props.user.lastName
    },
    userNickName() {
      return this.$props.user.nickName
    },
    userIsAdmin() {
      return this.$props.user.isAdmin
    },
    userIsSuperUser() {
      return this.$props.user.isSuperUser
    },
    userDateCreated() {
      return this.$props.user.dateCreated
    },
    userDateUpdated() {
      return this.$props.user.dateUpdated
    },
    userEmail() {
      return this.$props.user.email
    },
    userEmailConfirmed() {
      return this.$props.user.emailConfirmed
    }
  }
}
</script>
