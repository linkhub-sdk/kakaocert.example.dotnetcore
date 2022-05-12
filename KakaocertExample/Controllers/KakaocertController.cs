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
            /**
            * 전자서명을 요청합니다.
            */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // AppToApp 인증여부, true-AppToApp 방식, false-TalkMessage 방식
            bool isAppUseYN = false;

            RequestESign requestObj = new RequestESign();

            // 고객센터 전화번호, 카카오톡 인증메시지 중 "고객센터" 항목에 표시
            requestObj.CallCenterNum = "1600-8536";

            // 고객센터명, 카카오톡 인증메시지 중 "고객센터명" 항목에 표시
            requestObj.CallCenterName = "테스트";

            // 인증요청 만료시간(초), 최대값 1000, 인증요청 만료시간(초) 내에 미인증시 만료 상태로 처리됨
            requestObj.Expires_in = 60;

            // 수신자 생년월일, 형식 : YYYYMMDD
            requestObj.ReceiverBirthDay = "19700101";

            // 수신자 휴대폰번호
            requestObj.ReceiverHP = "010111222";

            // 수신자 성명
            requestObj.ReceiverName = "홍길동";

            // 별칭코드, 이용기관이 생성한 별칭코드 (파트너 사이트에서 확인가능)
            // 카카오톡 인증메시지 중 "요청기관" 항목에 표시
            // 별칭코드 미 기재시 이용기관의 이용기관명이 "요청기관" 항목에 표시
            // AppToApp 인증시 동작하지 않음.
            requestObj.SubClientID = "";

            // 인증요청 메시지 부가내용, 카카오톡 인증메시지 중 상단에 표시
            // AppToApp 인증시 동작하지 않음.
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
                var result = _kakaocertService.requestESign(clientCode, requestObj, isAppUseYN);
                return View("ResponseESign", result);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }
        }

        public IActionResult GetESignState()
        {
            /**
            * 전자서명 서명 상태를 합니다.
            */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // 요청시 반환받은 접수아이디
            string receiptId = "022050910164100001";

            try
            {
                var resultObj = _kakaocertService.getESignState(clientCode, receiptId);
                return View("GetESignState", resultObj);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }

        }

        public IActionResult VerifyESign()
        {
            /**
            * 전자서명 요청시 반환된 접수아이디를 통해 서명을 검증합니다.
            * - 서명검증시 전자서명 데이터 전문(signedData)이 반환됩니다.
            * - 카카오페이 서비스 운영정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류처리됩니다.
            */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // 요청시 반환받은 접수아이디
            string receiptId = "022050910164100001";

            // AppToApp 앱스킴 성공처리시 반환되는 서명값(iOS-sig, Android-signature)
            // Talk Message 인증시 null 기재하여 호출.
            string signature = null;

            try
            {
                var resultObj = _kakaocertService.verifyESign(clientCode, receiptId, signature);
                return View("ResponseVerify", resultObj);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }

        }



        public IActionResult RequestVerifyAuth()
        {
            /**
             * 본인인증을 요청합니다.
             */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            RequestVerifyAuth requestObj = new RequestVerifyAuth();

            // 고객센터 전화번호, 카카오톡 인증메시지 중 "고객센터" 항목에 표시
            requestObj.CallCenterNum = "1600-8536";

            // 고객센터명, 카카오톡 인증메시지 중 "고객센터명" 항목에 표시
            requestObj.CallCenterName = "테스트";

            // 인증요청 만료시간(초), 인증요청 만료시간(초) 내에 미인증시, 만료 상태로 처리됨
            requestObj.Expires_in = 60;

            // 수신자 생년월일, 형식 : YYYYMMDD
            requestObj.ReceiverBirthDay = "19700101";

            // 수신자 휴대폰번호
            requestObj.ReceiverHP = "010111222";

            // 수신자 성명
            requestObj.ReceiverName = "홍길동";

            // 별칭코드, 이용기관이 생성한 별칭코드 (파트너 사이트에서 확인가능)
            // 카카오톡 인증메시지 중 "요청기관" 항목에 표시
            // 별칭코드 미 기재시 이용기관의 이용기관명이 "요청기관" 항목에 표시
            requestObj.SubClientID = "";

            // 인증요청 메시지 부가내용, 카카오톡 인증메시지 중 상단에 표시
            requestObj.TMSMessage = "TMSMessage0423";

            // 인증요청 메시지 제목, 카카오톡 인증메시지 중 "요청구분" 항목에 표시
            requestObj.TMSTitle = "TMSTitle 0423";

            // 토큰 원문
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
                var result = _kakaocertService.requestVerifyAuth(clientCode, requestObj);
                return View("ReceiptID", result);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }
        }

        public IActionResult GetVerifyAuthState()
        {
            /**
            * 본인인증 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다.
            */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // 요청시 반환받은 접수아이디
            string receiptId = "022050910174500001";

            try
            {
                var resultObj = _kakaocertService.getVerifyAuthState(clientCode, receiptId);
                return View("GetVerifyAuthState", resultObj);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }

        }

        public IActionResult VerifyAuth()
        {
            /**
            * 본인인증 요청시 반환된 접수아이디를 통해 본인인증 서명을 검증합니다.
            * - 서명검증시 전자서명 데이터 전문(signedData)이 반환됩니다.
            * - 본인인증 요청시 작성한 Token과 서명 검증시 반환되는 signedData의 동일여부를 확인하여 본인인증 검증을 완료합니다.
            * - 카카오페이 서비스 운영정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류처리됩니다.
            */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // 요청시 반환받은 접수아이디
            string receiptId = "022050910174500001";

            try
            {
                var resultObj = _kakaocertService.verifyAuth(clientCode, receiptId);
                return View("ResponseVerify", resultObj);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }

        }

        public IActionResult RequestCMS()
        {
            /**
            *  자동이체 출금동의 서명을 요청합니다.
            */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // AppToApp 인증여부, true-AppToApp 방식, false-TalkMessage 방식
            bool isAppUseYN = false;

            RequestCMS requestObj = new RequestCMS();

            // 고객센터 전화번호, 카카오톡 인증메시지 중 "고객센터" 항목에 표시
            requestObj.CallCenterNum = "1600-8536";

            // 고객센터명, 카카오톡 인증메시지 중 "고객센터명" 항목에 표시
            requestObj.CallCenterName = "테스트";

            // 인증요청 만료시간(초), 인증요청 만료시간(초) 내에 미인증시, 만료 상태로 처리됨
            requestObj.Expires_in = 60;

            // 수신자 생년월일, 형식 : YYYYMMDD
            requestObj.ReceiverBirthDay = "19700101";

            // 수신자 휴대폰번호
            requestObj.ReceiverHP = "01011112222";

            // 수신자 성명
            requestObj.ReceiverName = "홍길동";


            // 예금주명	
            requestObj.BankAccountName = "예금주명";

            // 계좌번호, 이용기관은 사용자가 식별가능한 범위내에서 계좌번호의 일부를 마스킹 처리할 수 있음 (예시) 371-02-6***85
            requestObj.BankAccountNum = "9-4324-5**7-58";

            // 참가기관 코드
            requestObj.BankCode = "004";

            // 납부자번호, 이용기관에서 부여한 고객식별번호
            requestObj.ClientUserID = "clientUserID-0423-01";

            // 별칭코드, 이용기관이 생성한 별칭코드 (파트너 사이트에서 확인가능)
            // 카카오톡 인증메시지 중 "요청기관" 항목에 표시
            // 별칭코드 미 기재시 이용기관의 이용기관명이 "요청기관" 항목에 표시
            requestObj.SubClientID = "";

            // 인증요청 메시지 부가내용, 카카오톡 인증메시지 중 상단에 표시
            requestObj.TMSMessage = "TMSMessage0423";

            // 인증요청 메시지 제목, 카카오톡 인증메시지 중 "요청구분" 항목에 표시
            requestObj.TMSTitle = "TMSTitle 0423";


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
                var result = _kakaocertService.requestCMS(clientCode, requestObj, isAppUseYN);
                return View("ResponseCMS", result);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }
        }

        public IActionResult GetCMSState()
        {
            /**
            * 자동이체 출금동의 요청시 반환된 접수아이디를 통해 서명 상태를 확인합니다.
            */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // 요청시 반환받은 접수아이디
            string receiptId = "022050910184500001";

            try
            {
                var resultObj = _kakaocertService.getCMSState(clientCode, receiptId);
                return View("GetCMSState", resultObj);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }

        }

        public IActionResult VerifyCMS()
        {
            /**
            * 자동이체 출금동의 요청시 반환된 접수아이디를 통해 서명을 검증합니다.
            * - 서명검증시 전자서명 데이터 전문(signedData)이 반환됩니다.
            * - 카카오페이 서비스 운영정책에 따라 검증 API는 1회만 호출할 수 있습니다. 재시도시 오류처리됩니다.
            */

            // Kakaocert 이용기관코드, Kakaocert 파트너 사이트에서 확인
            string clientCode = "020040000001";

            // 요청시 반환받은 접수아이디
            string receiptId = "022050910184500001";

            // AppToApp 앱스킴 성공처리시 반환되는 서명값(iOS-sig, Android-signature)
            // Talk Message 인증시 null 기재하여 호출.
            string signature = null;

            try
            {
                var resultObj = _kakaocertService.verifyCMS(clientCode, receiptId, signature);
                return View("ResponseVerify", resultObj);
            }
            catch (KakaocertException ke)
            {
                return View("Exception", ke);
            }

        }

    }
}
