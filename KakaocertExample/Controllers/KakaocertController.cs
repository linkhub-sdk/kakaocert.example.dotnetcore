using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Kakaocert;

namespace KakaocertExample.Controllers
{
    public class KakaocertController : Controller
    {
        private readonly KakaocertService _kakaocertService;

        public KakaocertController(KakaocertInstance KCinstance)
        {
            //Kakaocert 서비스 객체 생성
            _kakaocertService = KCinstance.kakaocertService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RequestESign()
        {
            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            RequestESign requestObj = new RequestESign();

            // 고객센터 전화번호, 카카오톡 인증메시지 중 "고객센터" 항목에 표시
            requestObj.CallCenterNum = "1600-8536";

            // 인증요청 만료시간(초), 최대값 1000, 인증요청 만료시간(초) 내에 미인증시 만료 상태로 처리됨
            requestObj.Expires_in = 60;

            // 수신자 생년월일, 형식 : YYYYMMDD
            requestObj.ReceiverBirthDay = "19900108";

            // 수신자 휴대폰번호
            requestObj.ReceiverHP = "01043245117";

            // 수신자 성명
            requestObj.ReceiverName = "정요한";

            // 별칭코드, 이용기관이 생성한 별칭코드 (파트너 사이트에서 확인가능)
            // 카카오톡 인증메시지 중 "요청기관" 항목에 표시
            // 별칭코드 미 기재시 이용기관의 이용기관명이 "요청기관" 항목에 표시
            requestObj.SubClientID = "";

            // 인증요청 메시지 부가내용, 카카오톡 인증메시지 중 상단에 표시
            requestObj.TMSMessage = "TMSMessage0423";

            // 인증요청 메시지 제목, 카카오톡 인증메시지 중 "요청구분" 항목에 표시
            requestObj.TMSTitle = "TMSTitle 0423";

            // 전자서명할 토큰 원문
            requestObj.Token = "TMS Token 0423 ";

            // 은행계좌 실명확인 생략여부
            // true : 은행계좌 실명확인 절차를 생략
            // false : 은행계좌 실명확인 절차를 진행
            // 카카오톡 인증메시지를 수신한 사용자가 카카오인증 비회원일 경우, 카카오인증 회원등록 절차를 거쳐 은행계좌 실명확인 절차를 밟은 다음 전자서명 가능
            requestObj.isAllowSimpleRegistYN = false;

            // 수신자 실명확인 여부
            // true : 카카오페이가 본인인증을 통해 확보한 사용자 실명과 ReceiverName 값을 비교
            // false : 카카오페이가 본인인증을 통해 확보한 사용자 실명과 RecevierName 값을 비교하지 않음.
            requestObj.isVerifyNameYN = true;

            // PayLoad, 이용기관이 생성한 payload(메모) 값
            requestObj.PayLoad = "Payload123";

            try
            {
                var result = _kakaocertService.requestESign(clientCode, requestObj);
                return View("ReceiptID", result);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }
        }

        public IActionResult GetESignResult()
        {

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // 요청시 반환받은 접수아이디
            string receiptId = "020050710183000001";

            try
            {
                var resultObj = _kakaocertService.GetESignResult(clientCode, receiptId);
                return View("GetESignResult", resultObj);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }

        }


    }
}
