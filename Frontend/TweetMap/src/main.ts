
function drawState(field:CanvasRenderingContext2D,state:State){
  field.beginPath() 
  field.fillStyle="black"
  field.fillText(state.name,300,300)
  field.strokeStyle="black"
  field.moveTo(state.points[0].x,state.points[0].y)
  for(let i=1;i<state.points.length;i++)
  {
    field.lineTo(state.points[i].x,state.points[i].y)
  }
  field.stroke()
  field.fillStyle=getColor(state.value);
  field.fill()
  field.closePath();
}
function getColor(value:number|null){
  if(value==null)
    return "rgb(192,192,192)"
  const color=`rgb(${defaultColor+value*colorMultiplier},${defaultColor+Math.abs(value*colorMultiplier)},${defaultColor-value*colorMultiplier})`
  console.log(color)
  return color;
}
type State={
  points:Point[],
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
const colorMultiplier=128
const defaultColor=128
const field=canvas.getContext("2d") as CanvasRenderingContext2D
const input=document.getElementById("input") as HTMLInputElement
field.font="50px serif"

const fakePoints=[{x:100,y:100},{x:150,y:80},{x:200,y:120},{x:300,y:100},{x:600,y:50},{x:350,y:400},{x:120,y:600},{x:100,y:100},]
const state:State={
  points:fakePoints,
  name:"WC",
  value:null
}
input.addEventListener(('input'),()=>{
  state.value=input.valueAsNumber/50-1
  drawState(field,state)
})
drawState(field,state)
const newPoints=fakePoints.map((point)=>{
  const newPoint:Point={x:point.x+300,y:point.y+300}
  return newPoint
})
//drawState(field,{...state,points:newPoints})


