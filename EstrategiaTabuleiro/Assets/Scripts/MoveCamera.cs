using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    public float Speed = 1;  // A velocidade geral do movimento da câmera
    public float MinZoom, MaxZoom;
    public float LimitX, LimitZ;
    public Transform CameraRotator;  // O Objeto com nome :"CameraRotator" que está cuidando da rotação da câmera por meio de parentesco

	// Update is called once per frame
	void FixedUpdate () {

        CameraRotator.transform.Translate(Input.GetAxis("Horizontal") * Speed, -Input.GetAxis("Mouse ScrollWheel") * (Speed * 5) ,Input.GetAxis("Vertical") * Speed);

        //                       Movimento Horizontal                     Zoom                                            Movimento Vertical
        //Note que todos os valores estão multiplicados por Speed, ou seja, quanto maior speed, mais rápido a câmera age.

        if (this.transform.position.y > MaxZoom)
        {
            this.transform.position = new Vector3(this.transform.position.x, MaxZoom, this.transform.position.z);
        }

        if (this.transform.position.y < MinZoom)
        {
            this.transform.position = new Vector3(this.transform.position.x, MinZoom, this.transform.position.z);
        }

        if (this.transform.position.z > LimitZ)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, LimitZ);
        }

        if (this.transform.position.z < -LimitZ)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -LimitZ);
        }

        if (this.transform.position.x > LimitX)
        {
            this.transform.position = new Vector3(LimitX, this.transform.position.y, this.transform.position.z);
        }

        if (this.transform.position.x < -LimitX)
        {
            this.transform.position = new Vector3(-LimitX, this.transform.position.y, this.transform.position.z);
        }

        //Rotação da Câmera
        if ( Input.GetMouseButton( 1 ))
            // Aqui estou verificando se o mouse direito está apertado
        {
            CameraRotator.transform.Rotate(0, Input.GetAxis("Mouse X") , 0);
            // Pego o objeto ( Pai do movimentador da câmera ) e o rotaciono junto com o movimento horizontal do mouse
        }
	}
}
