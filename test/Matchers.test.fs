module Fable.Import.Jest.Test.Matchers

open Fable.Import
open Fable.Import.Jest
open Fable.Import.Jest.Matchers
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Node

test "it should have a toEqual function" <| fun () ->
  toEqual 1 1

test "it should have toEqual sugar" <| fun () ->
  1 == 1

test "it should have toBe sugar" <| fun () ->
  1 === 1

test "it should have a toBe function" <| fun () ->
  toBe 1 1

test "it should have a matcher" <| fun () ->
  let m = Matcher()

  m.Mock "1"

  m.CalledWith "1"

test "it should have a matcher2" <| fun () ->
  let m = Matcher2()

  m.Mock "1" "2"

  m.CalledWith "1" "2"

test "it should have a matcher3" <| fun () ->
  let m = Matcher3()

  m.Mock "1" "2" "3"

  m.CalledWith "1" "2" "3"

test "it should track calls for matcher2" <| fun () ->
  expect.assertions 3

  let m = Matcher2()

  m.Mock "0" "1"
  m.Mock "3" "4"

  toEqual m.Calls [|("0", "1"); ("3", "4")|]
  toEqual m.LastCall ("3", "4")
  m.LastCalledWith "3" "4"

test "it should track calls for matcher3" <| fun () ->
  expect.assertions 3

  let m = Matcher3()

  m.Mock "0" "1" "2"
  m.Mock "3" "4" "5"

  toEqual m.Calls [|("0", "1", "2"); ("3", "4", "5")|]
  toEqual m.LastCall ("3", "4", "5")
  m.LastCalledWith "3" "4" "5"

test "should work with matching some" <| fun () ->
  expect.assertions 2
  toEqualSome "3" (Some "3")
  toEqualNone None

describe "mocking external" <| fun () ->
  let mutable Net: obj = null
  let mutable mockMatcher: Matcher<string, float> = null

  beforeEach <| fun () ->
    mockMatcher <- Matcher<string, float>()

    jest.mock("net", fun () ->
      createObj ["isIP" ==> mockMatcher.Mock]
    )

    Net <- Fable.Import.Node.Globals.require.Invoke "net"

  test "should work with mocking external deps" <| fun () ->
    (Net :?> Node.Net.IExports).isIP("foo")
      |> ignore

    toBe "foo" mockMatcher.LastCall
