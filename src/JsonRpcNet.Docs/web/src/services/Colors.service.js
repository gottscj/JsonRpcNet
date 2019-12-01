export class ColorsService {
  serviceColors = [
    "#ADEFD1FF",
    "#86899C",
    "#D6ED17FF",
    "#ED2B33FF",
    "#A4CCFF",
    "#D85A7FFF",
    "#F95700FF",
    "#2C5F2D",
    "#00539CFF",
    "#97BC62FF",
    "#EEA47FFF"
  ];

  getColor(index) {
    return this.serviceColors[index % this.serviceColors.length];
  }
}
