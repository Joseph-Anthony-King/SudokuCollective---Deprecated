export function requestCheckGame(payload) {
  let matrix = [];

  payload.game.forEach((cell) => {
    if (cell !== "") {
      matrix.push(parseInt(cell));
    } else {
      matrix.push(0);
    }
  });

  const result = {
    firstRow: matrix.slice(0, 9),
    secondRow: matrix.slice(9, 18),
    thirdRow: matrix.slice(18, 27),
    fourthRow: matrix.slice(27, 36),
    fifthRow: matrix.slice(36, 45),
    sixthRow: matrix.slice(45, 54),
    seventhRow: matrix.slice(54, 63),
    eighthRow: matrix.slice(63, 72),
    ninthRow: matrix.slice(72, 81),
  };

  return result;
}
