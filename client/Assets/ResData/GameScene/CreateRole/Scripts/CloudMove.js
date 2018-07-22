#pragma strict

var Speed = 1;

function Start () {

}

function Update () {

   transform.Rotate(0, 0, Time.deltaTime * Speed * 0.1);
}