using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    public float Speed = 1;  // A velocidade geral do movimento da câmera
    public Transform CameraRotator;  // O Objeto com nome :"CameraRotator" que está cuidando da rotação da câmera por meio de parentesco

	// Update is called once per frame
	void Update () {

        this.transform.Translate(Input.GetAxis("Horizontal") * Speed, -Input.GetAxis("Mouse ScrollWheel") * (Speed * 5) ,Input.GetAxis("Vertical") * Speed);
	    //                       Movimento Horizontal                     Zoom                                            Movimento Vertical
        //Note que todos os valores estão multiplicados por Speed, ou seja, quanto maior speed, mais rápido a câmera age.

        //Rotação da Câmera
        if( Input.GetMouseButton( 1 ))
            // Aqui estou verificando se o mouse direito está apertado
        {
            CameraRotator.transform.Rotate(0, Input.GetAxis("Mouse X") , 0);
            // Pego o objeto ( Pai do movimentador da câmera ) e o rotaciono junto com o movimento horizontal do mouse
        }
	}
}
