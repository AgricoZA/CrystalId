module Tests

open NUnit.Framework
open System
open CrystalId

[<TestFixture>]
type TestClass () =
  [<Test>]
  member this.ReturnsId() = 

    let CrystalId = {
      epochStartYear = 2019
      sequenceBitCount = 2
      processBitCount = 10
      processNumber = 1L
      maximumBorrowedIdCount = 1L
      epochOffsetSeconds = 0L
      sequenceNumber = 0L
      id = 0L }

    let CrystalId2 = nextId CrystalId (DateTime(2019, 01, 01, 00, 00, 00))

    Assert.True(CrystalId2.id > 0L)

 // [<Test>]
 // member this.FailEveryTime() = Assert.True(false)

(*
[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
    Assert.Pass()

    *)
  (*

let private test = 

let mutable CrystalId = {
  epochStartYear = 2019
  sequenceBitCount = 2
  processBitCount = 10
  processNumber = 1L
  maximumBorrowedIdCount = 1L
  epochOffsetSeconds = 0L
  sequenceNumber = 0L
  id = 0L
}

CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,01));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,02));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,02));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,02));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId
CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
printfn "%A" CrystalId

*)