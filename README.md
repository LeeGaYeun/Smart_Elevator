# Smart_Elevator 프로젝트

## 프로젝트 개요

- **프로젝트 명**: 스마트 엘리베이터 프로젝트
- **시나리오**
    - 미리 개인정보 (층 수 등 필요한 정보와 얼굴 이미지)를 database에 저장합니다.
    - 실시간 웹캠과 database에 저장된 내용을 비교하여, 인증된 사용자일 경우에만 엘리베이터 모터를 구동합니다.
    - 인증에 실패하면 LCD에 실패 메시지를 표시하고, 이동 중에는 세그먼트에 현재 층 수를 표시합니다.
    - 저장된 층 수까지 이동 후에는 모터를 정지합니다.
    - 추후 개인정보를 Database에 저장하는 프로그램을 Winform으로 구현할 예정입니다.

## 작업순서

- [간트차트](https://www.notion.so/b30813db096c4118962241aaeaa532e9?pvs=21)
- [칸반보드](https://www.notion.so/cdf8a76bd70f43c8afe5fb01547a1afa?pvs=21)

## 시스템 다이어그램

![시퀀스 다이어그램](https://github.com/LeeGaYeun/Smart_Elevator/assets/149138767/6d268147-d11b-4afe-90c8-d9419692859a)

## 프로젝트 성과 결과

- 하드웨어 및 소프트웨어 통합
- 데이터베이스에 정보 저장 및 원하는 데이터 삭제
- 다중 스레드와 뮤텍스 사용
- 소켓 통신

## 아쉬운점

- 클라이언트 코드를 VS Code로 작성하여 실행 파일을 만들지 못했습니다.
- 엘리베이터를 하드웨어적으로 모터 정/역회전으로 구현하려 했으나 세그먼트로만 표현했습니다.
- 수동 조작을 고려한 keypad 연결을 하지 못했습니다.
- 소켓 통신 및 C#에서 OpenCV 구현에 부족한 스킬로 인해 시간이 지체되었습니다.
- 알고리즘 구현에 오랜 시간이 소요되었습니다.
- 얼굴 인식을 통해서만 엘리베이터가 이동합니다.
- 데이터베이스에 저장된 층과 1층만 이동할 수 있습니다.

## 참고

- [OpenCVSharp4 Github](https://github.com/opencv/opencv/tree/4.x)
- [GPIOZero Blog](https://blog.naver.com/emperonics/221831160948)
- [참고 블로그 1](https://luckygg.tistory.com/331)

## 시연

[Smart Elevator 시연 영상](https://youtu.be/PLULRvsDomk)
