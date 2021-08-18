class Difficulty {
  constructor(data) {
    if (data == undefined) {
      this.id = 0;
      this.name = "";
      this.displayName = "";
      this.difficultyLevel = 0;
    } else {
      this.id = data.id;
      this.name = data.name;
      this.displayName = data.displayName;
      this.difficultyLevel = data.difficultyLevel;
    }
  }
}

export default Difficulty;
