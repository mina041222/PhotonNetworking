using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fire : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;
    private ParticleSystem muzzleFlash;

    private PhotonView pv;
    //왼쪽 마우스 버튼 클릭 이벤트 저장
    private bool isMouseClick => Input.GetMouseButtonDown(0);

    // Start is called before the first frame update
    void Start()
    {
        //포톤뷰 컴포넌트 연결
        pv = GetComponent<PhotonView>();
        //FirePos 하위에 있는 총구 화염 효과 연결
        muzzleFlash = firePos.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //로컬 유저 여부와 마우스 왼쪽 버튼을 클릭했을 때 총알을 발사
        if(pv.IsMine && isMouseClick)
        {
            FireBullet();
            //RPC로 원격지에 있는 함수를 호출
            pv.RPC("FireBullet", RpcTarget.Others, null);
        }
    }

    [PunRPC]
    void FireBullet()
    {
        if(!muzzleFlash.isPlaying) muzzleFlash.Play(true);

        GameObject bullet = Instantiate(bulletPrefab,
                                        firePos.position,
                                        firePos.rotation);
    }
}
