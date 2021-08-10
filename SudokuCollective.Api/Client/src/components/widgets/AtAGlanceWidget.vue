<template>
  <div v-if="!processing">
    <v-card elevation="6" class="mx-auto">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center">At a Glance</v-card-title>
          <hr class="title-spacer" />
          <div class="app-buttons-scroll">
            <ProdToDevProgressGauge />
            <CustomEmailConfirmationSetupGauge />
            <CustomPasswordResetSetupGauge />
            <AdminToUserProgressGauge />
          </div>
        </v-container>
      </v-card-text>
    </v-card>
    <div class="card-spacer"></div>
  </div>
</template>

<style></style>

<script>
/* eslint-disable no-unused-vars */
import { mapGetters } from "vuex";
import User from "@/models/user";
import AdminToUserProgressGauge from "@/components/widgets/gauges/AdminToUserProgressGauge";
import CustomEmailConfirmationSetupGauge from "@/components/widgets/gauges/CustomEmailConfirmationSetupGauge";
import CustomPasswordResetSetupGauge from "@/components/widgets/gauges/CustomPasswordResetSetupGauge";
import ProdToDevProgressGauge from "@/components/widgets/gauges/ProdToDevProgressGauge";

export default {
  name: "AtAGlanceWidget",
  components: {
    AdminToUserProgressGauge,
    CustomEmailConfirmationSetupGauge,
    CustomPasswordResetSetupGauge,
    ProdToDevProgressGauge,
  },
  data: () => ({
    user: new User(),
    processing: false,
  }),
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
  },
  created() {
    this.$data.processing = true;
    this.$data.user = this.getUser;
    this.$data.processing = false;
  },
};
</script>
