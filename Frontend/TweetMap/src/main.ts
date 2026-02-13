
function drawPoligon(field:CanvasRenderingContext2D,points:Point[]){
  field.beginPath() 
  field.strokeStyle="black"
  field.moveTo(points[0].x,points[0].y)
  for(let i=1;i<points.length;i++)
  {
    field.lineTo(points[i].x,points[i].y)
  }
  field.stroke()
  field.fill()
  field.closePath();
}
function getAverageCoords(points:Point[]){
  let averageX=0,averageY=0
  points.forEach((point)=>{
    averageX+=point.x;
    averageY+=point.y
  })
  return [averageX/points.length,averageY/points.length]
}
function drawState(state:State,field:CanvasRenderingContext2D){
  field.fillStyle=getColor(state.value);
  state.points.forEach((points)=>{
    drawPoligon(field,points)
  })
  field.fillStyle="black"
  const [x,y]=getAverageCoords(state.points[0])
  field.fillText(state.name,x,y)
}
function getColor(value:number|null){
  if(value==null)
    return "rgb(192,192,192)"
  const red=Math.min(defaultColor+value*colorMultiplier*2,256)
  const green=Math.min(defaultColor+Math.abs(value*colorMultiplier),256)
  const blue=Math.min(defaultColor-value*colorMultiplier*1.5,256)
  const color=`rgb(${red},${green},${blue})`
  console.log(color)
  return color;
}
type State={
  points:Point[][],
  name:string,
  value:number|null
}
type Point={
  x:number,
  y:number,
}
type TweetPoint=Point&{
  value:number
}

const canvas=document.getElementById("canvas") as HTMLCanvasElement
const colorMultiplier=200
const defaultColor=128
const field=canvas.getContext("2d") as CanvasRenderingContext2D
const input=document.getElementById("input") as HTMLInputElement
field.font="50px serif"

const fakePoints=[{x:100,y:100},{x:150,y:80},{x:200,y:120},{x:300,y:100},{x:600,y:50},{x:350,y:400},{x:120,y:600},{x:100,y:100},]
const state:State={
  points:[fakePoints,[{x:800,y:800},{x:800,y:400},{x:400,y:0},{x:800,y:800}]],
  name:"WC",
  value:null
}
input.addEventListener(('input'),()=>{
  state.value=input.valueAsNumber/50-1
  drawState(state,field)
})
drawState(state,field)
/*const newPoints=fakePoints.map((point)=>{
  const newPoint:Point={x:point.x+300,y:point.y+300}
  return newPoint
})
drawState(field,{...state,points:newPoints})*/


